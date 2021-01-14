using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GestionAmbiental.Application.UseCases.SGA_Documentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.Items;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarRevisionDocumento;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarPredecesores;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Application.UseCases.SGA_Documentos.EditarRevisionDocumento;

namespace GestionAmbiental.UseCases.SGA_Documentos.Doumentacion
{
    public class SGADocumentacionController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IConsultarDocumentos _consultarDocumentos;
        private readonly IConsultarDocumento _consultarDocumento;
        private readonly IConsultarPredecesores _consultarPredecesores;
        private readonly IConsultarRevisionDocumento _consultarRevisionDocumento;
        private readonly IEditarRevisionDocumento _editarRevisionDocumento;
        private readonly IGetUsuario _getUsuario;

        private readonly IDataProtector _protector;

        public SGADocumentacionController(
            ISelectItems selectItems,
            IConsultarDocumentos consultarDocumentos,
            IConsultarDocumento consultarDocumento,
            IConsultarPredecesores consultarPredecesores,
            IConsultarRevisionDocumento consultarRevisionDocumento, 
            IEditarRevisionDocumento editarRevisionDocumento,
            IGetUsuario getUsuario,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _selectItems = selectItems;
            _consultarDocumentos = consultarDocumentos;
            _consultarPredecesores = consultarPredecesores;
            _consultarDocumento = consultarDocumento;
            _consultarRevisionDocumento = consultarRevisionDocumento;
            _editarRevisionDocumento = editarRevisionDocumento;
            _getUsuario = getUsuario;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.DocumentosSGAIdRouteValue);
        }
        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipos = items.ItemsTipoDocumento.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGATipoDocumento.ToString(), Text = g.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.TiposDocJson = JsonConvert.SerializeObject(items.ItemsTipoDocumento); 

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string nombre, int tipo, string codigo, int condicion, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute(nombre, tipo, codigo, condicion, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Documentos)
                {
                    if (fila.Estado != EstadosDocumentosSGA.Pendiente.ToString())
                    {
                        var id_encriptado = _protector.Protect(fila.IdSGADocumento.ToString());
                        filasTabla += "<tr>";
                        filasTabla += "<td><a href=\"/SGADocumentacion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
                        filasTabla += "<td>" + fila.DocNombre + "</td>";
                        filasTabla += "<td>" + fila.DocVersion + "</td>";
                        filasTabla += "<td>" + fila.FechaAprobacion + "</td>";
                        filasTabla += "<td>" + fila.Condicion + "</td>";
                        filasTabla += "<td>" + fila.Comentario + "</td>";
                        filasTabla += "<td>" + fila.FechaTentativa + "</td>";
                        filasTabla += "</tr>";
                    }
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                filasTabla = "Sin datos";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetTablaModalResumenFiltrada(int tipo, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute("", tipo, "", 0, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Documentos)
                {
                    var id_encriptado = _protector.Protect(fila.IdSGADocumento.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/SGADocumentacion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
                    filasTabla += "<td>" + fila.DocNombre + "</td>";
                    filasTabla += "<td>" + fila.DocVersion + "</td>";
                    filasTabla += "<td>" + fila.FechaAprobacion + "</td>";
                    filasTabla += "<td>" + fila.Condicion + "</td>";
                    filasTabla += "<td>" + fila.Comentario + "</td>";
                    filasTabla += "<td>" + fila.FechaTentativa + "</td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrarModal(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrarModal(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrarModal(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrarModal(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                filasTabla = "Sin datos";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> VerDocumento(string IdDocumento)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var usuarioActual = await _getUsuario.Execute(userInfo.NombreUsuario);

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipos = items.ItemsTipoDocumento.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGATipoDocumento.ToString(), Text = g.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;

                //Obtener ultimas versiones de documentos publicados
                var documentos = await _consultarDocumentos.Execute(IdDesencriptado);
                var documentosAgrupados = documentos.GroupBy(x => x.SGATipoDocumento).ToList();

                var predecesoresTipo = new List<SelectListItem>();
                foreach (var grupo in documentosAgrupados)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Key, Disabled = false };
                    foreach (var docPredecesor in grupo)
                        predecesoresTipo.Add(new SelectListItem() { Value = docPredecesor.IdSGADocumento.ToString(), Text = "(" + docPredecesor.DocCodigo + ") " + docPredecesor.DocNombre + " v." + docPredecesor.DocVersion.ToString(), Group = nuevoGrupo });
                }
                predecesoresTipo.Insert(0, new SelectListItem("Ninguno", "0", true));

                ViewBag.Predecesores = predecesoresTipo;

                var documento = await _consultarDocumento.Execute(IdDesencriptado);

                var revisores = documento.Revisores.Select(x => new {
                    Id = _protector.Protect(x.IdSGARevisionDocumento.ToString()),
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"),
                    Estado = x.Estado,
                    Permiso = x.Usuario.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false
                })
                    .ToList();

                var aprobadores = documento.Aprobadores.Select(x => new {
                    Id = _protector.Protect(x.IdSGARevisionDocumento.ToString()),
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"),
                    Estado = x.Estado,
                    Permiso = x.Usuario.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false
                })
                    .ToList();

                var model = new SGADocumentoModel
                {
                    Id_Encriptado = IdDocumento,
                    Id_SGA_Documento = documento.IdSGADocumento,
                    Id_SGA_Tipo_Documento = documento.IdSGATipoDocumento,
                    Doc_Nombre = documento.DocNombre,
                    Doc_Predecesor = documento.IdDocPredecesor,
                    Doc_Version = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocumentoRuta = documento.DocRuta,
                    Revisores = JsonConvert.SerializeObject(revisores),
                    Aprobadores = JsonConvert.SerializeObject(aprobadores)
                };

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ConsultarPredecesores(string IdDocumento)
        {
            var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
            var datosTabla = await _consultarPredecesores.Execute(IdDesencriptado);
            var filasTabla = "";
            if (datosTabla.Count > 0)
            {
                foreach (var fila in datosTabla)
                {
                    var id_encriptado = _protector.Protect(fila.IdSGADocumento.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/SGADocumentacion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
                    filasTabla += "<td>" + fila.DocNombre + "</td>";
                    filasTabla += "<td>" + fila.DocVersion + "</td>";
                    filasTabla += "<td>" + fila.FechaAprobacion + "</td>";
                    filasTabla += "<td>" + fila.Condicion + "</td>";
                    filasTabla += "<td>" + fila.Comentario + "</td>";
                    filasTabla += "<td>" + fila.FechaTentativa + "</td>";
                    filasTabla += "</tr>";
                }
            }
            else
            {
                filasTabla = "Sin datos";
            }
            dynamic response = new
            {
                Data = filasTabla
            };
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> RevisarDocumento(string IdDocumentoRevision, string Comentario, string IdDocumento, string Opcion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var IdDocumentoRevisionInt = Int32.Parse(_protector.Unprotect(IdDocumentoRevision));
                var IdDocumentoInt = Int32.Parse(_protector.Unprotect(IdDocumento));
                //Validar que el usuario revisor del IdDocumentoRevisionInt es el usuario conectado
                var documentoRevision = await _consultarRevisionDocumento.Execute(IdDocumentoRevisionInt);
                var usuarioActual = await _getUsuario.Execute(userInfo.NombreUsuario);
                dynamic response;
                if (!documentoRevision.Usuario.Equals(usuarioActual.IdUsuario))
                {
                    response = new
                    {
                        Result = "Error",
                        Message = "El usuario actualmente conectado no es el mismo al usuario asignado a esta revisión"
                    };
                    return Ok(response);
                }
                else
                {
                    //Validar si se puede enviar la variable "documentoRevision" a la notificacion, incluir comentario
                    /*
                    if (Opcion.Equals("Rechazado"))
                    {
                        //Notificar
                        //Editar
                    }
                    else
                    {
                        //Editar
                        //Notificar
                    }
                    */
                    await _editarRevisionDocumento.Execute(IdDocumentoInt, IdDocumentoRevisionInt, Comentario, Opcion);

                    response = new
                    {
                        Result = "Ok",

                    };
                    return Ok(response);
                }
            }
        }

    }
}
