using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarPredecesores;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Application.UseCases.SGA_Documentos.EditarDocumento;
using System.IO;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using Renci.SshNet;
using GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.FirmarDocumento;

namespace GestionAmbiental.UseCases.SGA_Documentos.Administracion
{
    public class SGAAdministracionController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IConsultarDocumentos _consultarDocumentos;
        private readonly IConsultarDocumento _consultarDocumento;
        private readonly IConsultarPredecesores _consultarPredecesores;
        private readonly IConsultarRevisionDocumento _consultarRevisionDocumento;
        private readonly IGetUsuario _getUsuario;
        private readonly IEditarDocumento _editarDocumento;
        private readonly IRegistrarDocumento _registrarDocumento;
        private readonly IFirmarDocumento _firmarDocumento;

        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        public SGAAdministracionController(
                    ISelectItems selectItems,
                    IConsultarDocumentos consultarDocumentos,
                    IConsultarDocumento consultarDocumento,
                    IConsultarPredecesores consultarPredecesores,
                    IConsultarRevisionDocumento consultarRevisionDocumento,
                    IGetUsuario getUsuario,
                    IEditarDocumento editarDocumento,
                    IGetInfo_SGA info_SGA,
                    IRegistrarDocumento registrarDocumento, 
                    IFirmarDocumento firmarDocumento,
                    IDataProtectionProvider dataProtectionProvider,
                    DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _selectItems = selectItems;
            _consultarDocumentos = consultarDocumentos;
            _consultarPredecesores = consultarPredecesores;
            _consultarDocumento = consultarDocumento;
            _consultarRevisionDocumento = consultarRevisionDocumento;
            _getUsuario = getUsuario;
            _info_SGA = info_SGA;
            _registrarDocumento = registrarDocumento;
            _firmarDocumento = firmarDocumento;
            _editarDocumento = editarDocumento;
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
                

                var tipos = items.ItemsTipoDocumento.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGATipoDocumento.ToString(), Text = g.Descripcion }).ToList();
              
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.TiposDocJson = JsonConvert.SerializeObject(items.ItemsTipoDocumento);

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string nombre, int categoria, int[] tipos, string codigo, int[] condiciones, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute(nombre, categoria, tipos, codigo, condiciones, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Documentos)
                {
                    var id_encriptado = _protector.Protect(fila.IdSGADocumento.ToString());
                    filasTabla += "<tr>";
                    if (!fila.Estado.Equals(EstadosDocumentosSGA.Pendiente.ToString()) && !fila.Estado.Equals(EstadosDocumentosSGA.Firmas.ToString()))
                        filasTabla += "<td><a href=\"/SGAAdministracion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
                    else if (fila.Estado.Equals(EstadosDocumentosSGA.Firmas.ToString()))
                        filasTabla += "<td><a href=\"/SGAAdministracion/FirmarDocumento?IdDocumento=" + id_encriptado + "&&IdCategoria="+fila.IdSGACategoriaDocumento+ "\">" + fila.DocCodigo + "</a></td>";
                    else {
                        if (fila.IdDocPrincipal.HasValue || !fila.IdSGACategoriaDocumento.Equals(1))
                            filasTabla += "<td><a href=\"/SGAAdministracion/EditarDocumentoSRevision?IdDocumento=" + id_encriptado + "&&IdCategoria="+fila.IdSGACategoriaDocumento+"\">" + fila.DocCodigo + "</a></td>";
                        else
                            filasTabla += "<td><a href=\"/SGAAdministracion/EditarDocumento?IdDocumento=" + id_encriptado + "&&IdCategoria=" + fila.IdSGACategoriaDocumento + "\">" + fila.DocCodigo + "</a></td>";
                    }
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
        public async Task<IActionResult> GetTablaModalResumenFiltrada(int categoria, int[] condiciones, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute("",categoria, new int[0], "", condiciones, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Documentos)
                {
                    var id_encriptado = _protector.Protect(fila.IdSGADocumento.ToString());
                    filasTabla += "<tr>";
                    if (!fila.Estado.Equals(EstadosDocumentosSGA.Pendiente.ToString()) && !fila.Estado.Equals(EstadosDocumentosSGA.Firmas.ToString()))
                        filasTabla += "<td><a href=\"/SGAAdministracion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
                    else if (fila.Estado.Equals(EstadosDocumentosSGA.Firmas.ToString()))
                        filasTabla += "<td><a href=\"/SGAAdministracion/FirmarDocumento?IdDocumento=" + id_encriptado + "&&IdCategoria=" + fila.IdSGACategoriaDocumento + "\">" + fila.DocCodigo + "</a></td>";
                    else
                    {
                        if (fila.IdDocPrincipal.HasValue &&!fila.IdSGACategoriaDocumento.Equals(1))
                            filasTabla += "<td><a href=\"/SGAAdministracion/EditarDocumentoSRevision?IdDocumento=" + id_encriptado + "&&IdCategoria=" + fila.IdSGACategoriaDocumento + "\">" + fila.DocCodigo + "</a></td>";
                        else
                            filasTabla += "<td><a href=\"/SGAAdministracion/EditarDocumento?IdDocumento=" + id_encriptado + "&&IdCategoria=" + fila.IdSGACategoriaDocumento + "\">" + fila.DocCodigo + "</a></td>";
                    }
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

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var categoriasTipos = items.ItemsCategoriaTipo.Where(x => x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(documento.IdSGACategoriaDocumento)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriasTipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                //Obtener documentos que tienen como documento principal el actual
                var documentosSecundarios = await _consultarDocumentos.ExecuteSecundarios(IdDesencriptado);
                var secundarios = documentosSecundarios.Select(x => new
                {
                    IdEncriptado = _protector.Protect(x.IdSGADocumento.ToString()),
                    Codigo = x.DocCodigo,
                    TipoDocumento = x.SGATipoDocumento,
                    Nombre = x.DocNombre,
                    Version = x.DocVersion,
                    Condicion = x.Condicion,
                    Ruta = x.DocRuta
                }).ToList();

                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = categoriasTipos;

                var model = new SGADocumentoModel
                {
                    Id_Encriptado = IdDocumento,
                    Id_SGA_Documento = documento.IdSGADocumento,
                    Id_SGA_Tipo_Documento = documento.IdSGACategoriaTipo,
                    Doc_Nombre = documento.DocNombre,
                    Doc_Predecesor = documento.IdDocPredecesor,
                    Doc_Codigo = documento.DocCodigo,
                    Doc_Version = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocumentoRuta = documento.DocRuta,
                    Revisores = JsonConvert.SerializeObject(revisores),
                    Aprobadores = JsonConvert.SerializeObject(aprobadores),
                    Secundarios = JsonConvert.SerializeObject(secundarios)
                };

                return View(model);
            }
        }         
        //Solo Documentos SGA
        public async Task<IActionResult> CargarDocumento(string Error, int IdCategoria)
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

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SGI"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }
                ViewBag.CategoriaDoc = tituloCategoria;

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                //Solo documentos SGA
                var categoriastipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).OrderBy(y => y.TipoDocumento.Descripcion).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriastipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                
                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = categoriastipos;
                ViewBag.Usuarios = usuarios;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                                
                ViewBag.Error = Error;


                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> CargarDocumento(SGADocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.Id_SGA_Tipo_Documento));
                var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre + "_v." + String.Format("{0:0.00}", model.Doc_Version) + Path.GetExtension(model.Documento.FileName);
                var revisores = model.Revisores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores) : new List<RevisionAprobacionInputOutput>();
                var aprobadores = model.Aprobadores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores) : new List<RevisionAprobacionInputOutput>();
                var cargaDocumento = await CargarDocumento(model.Documento, nombreDoc, tipoDoc.TipoDocumento.Descripcion);

                if (!cargaDocumento.Equals("Error"))
                {
                    var idDocumento = await _registrarDocumento.Execute(
                        model.Id_SGA_Tipo_Documento,
                        model.Doc_Codigo,
                        model.Doc_Principal,
                        model.Doc_Nombre,
                        model.Doc_Version,
                        model.Doc_Predecesor,
                        model.Comentario,
                        cargaDocumento,
                        userInfo.NombreUsuario,
                        model.Opcion,
                        revisores,
                        aprobadores);
                    if (!model.Opcion.Equals("AgregarFormato"))
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()) });
                    else
                        //Falta IdCategoria
                        return RedirectToAction("CargarDocumentoSRevision", new { IdDocPrincipal = idDocumento, IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento});
                }
                else
                {
                    return RedirectToAction("CargarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
                }
            }
            return View(model);
        }
        //Documentos SGI y externos. Documentos secundarios (Formatos)
        public async Task<IActionResult> CargarDocumentoSRevision(string Error, int IdCategoria, int IdDocPrincipal)
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
                ViewBag.DocPrincipal = IdDocPrincipal;

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SGI"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }
                ViewBag.CategoriaDoc = tituloCategoria;

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.Usuarios = usuarios;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                                
                ViewBag.Error = Error;

                return View();
            }
        }        
        [HttpPost]
        public async Task<IActionResult> CargarDocumentoSRevision(SGADocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.Id_SGA_Tipo_Documento));
                var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre + "_v." + String.Format("{0:0.00}", model.Doc_Version) + Path.GetExtension(model.Documento.FileName);
                var revisores = model.Revisores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores) : new List<RevisionAprobacionInputOutput>();
                var aprobadores = model.Aprobadores!=null?JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores):new List<RevisionAprobacionInputOutput>();
                var cargaDocumento = await CargarDocumento(model.Documento, nombreDoc, tipoDoc.TipoDocumento.Descripcion);

                if (!cargaDocumento.Equals("Error"))
                {
                    var idDocumento = await _registrarDocumento.Execute(
                        model.Id_SGA_Tipo_Documento,
                        model.Doc_Codigo,
                        model.Doc_Principal,
                        model.Doc_Nombre,
                        model.Doc_Version,
                        model.Doc_Predecesor,
                        model.Comentario,
                        cargaDocumento,
                        userInfo.NombreUsuario,
                        model.Opcion,
                        revisores,
                        aprobadores);
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()) });
                }
                else
                {
                    return RedirectToAction("CargarDocumentoSRevision", new { Error = "Ocurrió un error al cargar el archivo" });
                }
            }
            return View(model);
        }
        //Solo Documentos SGA
        public async Task<IActionResult> ActualizarDocumento(string Error, int IdCategoria)
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
                //Obtener ultimas versiones de documentos publicados o obsoletos
                var documentos = await _consultarDocumentos.ExecutePublicados();
                //Filtrar documentos por categoria
                documentos = documentos.Where(x => x.IdSGACategoriaDocumento.Equals(IdCategoria)).ToList();
                //Listar las CategoriasTipos de documentos existentes
                var tiposDocExistentes = documentos.Select(x => x.IdSGACategoriaTipo).ToList();
                //Solo documentos SGA
                var categoriasTipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria) && tiposDocExistentes.Contains(x.IdSGACategoriaTipo)).OrderBy(y => y.TipoDocumento.Descripcion).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriasTipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var documentosPublicados = JsonConvert.SerializeObject(documentos);

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SGI"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }

                ViewBag.DocumentosPublicados = documentosPublicados;
                ViewBag.CategoriaDoc = tituloCategoria;
                ViewBag.Error = Error;
                ViewBag.TiposDoc = categoriasTipos;

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                
                ViewBag.DocumentosPublicados = documentosPublicados;
                ViewBag.CategoriaDoc = tituloCategoria;
                ViewBag.Error = Error;
                ViewBag.TiposDoc = categoriasTipos;
                ViewBag.Usuarios = usuarios;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarDocumento(SGADocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.Id_SGA_Tipo_Documento));
                var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre + "_v." + String.Format("{0:0.00}", model.Doc_Version) + Path.GetExtension(model.Documento.FileName);
                var revisores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores);
                var aprobadores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores);
                var cargaDocumento = await CargarDocumento(model.Documento, nombreDoc, tipoDoc.TipoDocumento.Descripcion);

                if (!cargaDocumento.Equals("Error"))
                {
                    var idDocumento = await _registrarDocumento.Execute(
                        model.Id_SGA_Tipo_Documento,
                        model.Doc_Codigo,
                        model.Doc_Principal,
                        model.Doc_Nombre,
                        model.Doc_Version,
                        model.Doc_Predecesor,
                        model.Comentario,
                        cargaDocumento,
                        userInfo.NombreUsuario,
                        model.Opcion,
                        revisores,
                        aprobadores);
                    if (!model.Opcion.Equals("AgregarFormato"))
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()) });
                    else
                        //Falta IdCategoria
                        return RedirectToAction("CargarDocumentoSRevision", new { IdDocPrincipal = idDocumento });
                }
                else
                {
                    return RedirectToAction("CargarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
                }
            }
            return View(model);
        }
        //Documentos SGI y externos.
        public async Task<IActionResult> ActualizarDocumentoSRevision(string Error, int IdCategoria, int IdDocPrincipal)
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
                ViewBag.DocPrincipal = IdDocPrincipal;

                var items = await _selectItems.Execute();
                //Obtener ultimas versiones de documentos publicados o obsoletos
                var documentos = await _consultarDocumentos.ExecutePublicados();
                //Filtrar documentos por categoria
                documentos = documentos.Where(x => x.IdSGACategoriaDocumento.Equals(IdCategoria)).ToList();
                //Listar las CategoriasTipos de documentos existentes
                var tiposDocExistentes = documentos.Select(x => x.IdSGACategoriaTipo).ToList();
                //Solo documentos SGA
                var categoriasTipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria) && tiposDocExistentes.Contains(x.IdSGACategoriaTipo)).OrderBy(y => y.TipoDocumento.Descripcion).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriasTipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var documentosPublicados = JsonConvert.SerializeObject(documentos);

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SGI"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }                  

                ViewBag.DocumentosPublicados = documentosPublicados;
                ViewBag.CategoriaDoc = tituloCategoria;
                ViewBag.Error = Error; 
                ViewBag.TiposDoc = categoriasTipos;

                return View();
            }
        }
        public async Task<IActionResult> EditarDocumento(string IdDocumento, int IdCategoria, string Error)
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

                var tipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

                ViewBag.Usuarios = usuarios;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                var documento = await _consultarDocumento.Execute(IdDesencriptado);
                //Obtener documentos que tienen como documento principal el actual
                var documentosSecundarios = await _consultarDocumentos.ExecuteSecundarios(IdDesencriptado);
                var secundarios = documentosSecundarios.Select(x => new
                {
                    IdEncriptado = _protector.Protect(x.IdSGADocumento.ToString()),
                    Codigo = x.DocCodigo,
                    TipoDocumento = x.SGATipoDocumento,
                    Nombre = x.DocNombre,
                    Version = x.DocVersion,
                    Condicion = x.Condicion,
                    Ruta = x.DocRuta
                }).ToList();

                var revisores = documento.Revisores.Select(x => new {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd")
                })
                    .ToList();

                var aprobadores = documento.Aprobadores.Select(x => new {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"),
                })
                    .ToList();

                var model = new SGADocumentoModel
                {
                    Id_Encriptado = IdDocumento,
                    Id_SGA_Documento = documento.IdSGADocumento,
                    Id_Categoria = IdCategoria,
                    Id_SGA_Tipo_Documento = documento.IdSGACategoriaTipo,
                    Doc_Nombre = documento.DocNombre,
                    Doc_Predecesor = documento.IdDocPredecesor,
                    Doc_Codigo = documento.DocCodigo,
                    Doc_Version = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocumentoRuta = documento.DocRuta,
                    Revisores = JsonConvert.SerializeObject(revisores),
                    Aprobadores = JsonConvert.SerializeObject(aprobadores),
                    Secundarios = JsonConvert.SerializeObject(secundarios)
                };

                ViewBag.Error = Error;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarDocumento(SGADocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.Id_SGA_Tipo_Documento));
                var revisores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>("[]");
                var aprobadores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>("[]");

                if (model.Revisores != null)
                    revisores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores);
                if (model.Aprobadores != null)
                    aprobadores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores);

                if (model.Documento != null)
                {
                    var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre + "_v." + String.Format("{0:0.00}", model.Doc_Version) + Path.GetExtension(model.Documento.FileName);
                    var cargaDocumento = await CargarDocumento(model.Documento, nombreDoc, tipoDoc.TipoDocumento.Descripcion);
                    if (!cargaDocumento.Equals("Error"))
                    {
                        await DeleteFTPFileAsync(model.DocumentoRuta);
                        await _editarDocumento.Execute(
                            model.Id_SGA_Documento,
                            model.Id_SGA_Tipo_Documento,
                            model.Doc_Codigo,
                            model.Doc_Nombre,
                            model.Doc_Version,
                            model.Doc_Predecesor,
                            model.Comentario,
                            cargaDocumento,
                            userInfo.NombreUsuario,
                            model.Opcion,
                            revisores,
                            aprobadores);
                        if (model.Opcion.Equals("Registrar") && !model.Opcion.Equals("AgregarFormato"))
                            return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()) });
                        else if (model.Opcion.Equals("AgregarFormato"))
                            return RedirectToAction("CargarDocumentoSRevision", new { IdDocPrincipal = model.Id_SGA_Documento, IdCategoria = model.Id_Categoria });
                        else
                            return RedirectToAction("EditarDocumento", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()), IdCategoria = model.Id_Categoria });
                    }
                    return RedirectToAction("EditarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
                }
                else
                {
                    await _editarDocumento.Execute(
                            model.Id_SGA_Documento,
                            model.Id_SGA_Tipo_Documento,
                            model.Doc_Codigo,
                            model.Doc_Nombre,
                            model.Doc_Version,
                            model.Doc_Predecesor,
                            model.Comentario,
                            model.DocumentoRuta,
                            userInfo.NombreUsuario,
                            model.Opcion,
                            revisores,
                            aprobadores);
                    if (model.Opcion.Equals("Registrar"))
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()) });
                    else if (model.Opcion.Equals("AgregarFormato"))
                        return RedirectToAction("CargarDocumentoSRevision", new { IdDocPrincipal = model.Id_SGA_Documento, IdCategoria = model.Id_Categoria });
                    else
                        return RedirectToAction("EditarDocumento", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()), IdCategoria = model.Id_Categoria });
                }

            }
            return View(model);
        }
        public async Task<IActionResult> EditarDocumentoSRevision(string IdDocumento, int IdCategoria, string Error)
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

                var tipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

                ViewBag.Usuarios = usuarios;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                //Obtener ultimas versiones de documentos publicados
                var documentosPredecesores = await _consultarDocumentos.ExecutePredecesores(IdDesencriptado);
                var documentosAgrupados = documentosPredecesores.GroupBy(x => x.IdSGACategoriaTipo).ToList();

                var predecesoresTipo = new List<SelectListItem>();
                foreach (var grupo in documentosAgrupados)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = "(" + grupo.First().SGACategoriaDocumento + ")" + grupo.First().SGATipoDocumento, Disabled = false };
                    foreach (var docPredecesor in grupo)
                        predecesoresTipo.Add(new SelectListItem() { Value = docPredecesor.IdSGADocumento.ToString(), Text = "(" + docPredecesor.DocCodigo + ") " + docPredecesor.DocNombre + " v." + docPredecesor.DocVersion.ToString(), Group = nuevoGrupo });
                }
                predecesoresTipo.Insert(0, new SelectListItem("Ninguno", "0", true));

                ViewBag.Predecesores = predecesoresTipo;

                var documento = await _consultarDocumento.Execute(IdDesencriptado);
                //Obtener documentos que tienen como documento principal el actual
                var documentosSecundarios = await _consultarDocumentos.ExecuteSecundarios(IdDesencriptado);
                var secundarios = documentosSecundarios.Select(x => new
                {
                    IdEncriptado = _protector.Protect(x.IdSGADocumento.ToString()),
                    Codigo = x.DocCodigo,
                    TipoDocumento = x.SGATipoDocumento,
                    Nombre = x.DocNombre,
                    Version = x.DocVersion,
                    Condicion = x.Condicion,
                    Ruta = x.DocRuta
                }).ToList();

                ViewBag.Secundarios = secundarios;

                var revisores = documento.Revisores.Select(x => new {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd")
                })
                    .ToList();

                var aprobadores = documento.Aprobadores.Select(x => new {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"),
                })
                    .ToList();

                var model = new SGADocumentoModel
                {
                    Id_Encriptado = IdDocumento,
                    Id_SGA_Documento = documento.IdSGADocumento,
                    Doc_Codigo = documento.DocCodigo,
                    Id_Categoria = IdCategoria,
                    Id_SGA_Tipo_Documento = documento.IdSGACategoriaTipo,
                    Doc_Nombre = documento.DocNombre,
                    Doc_Predecesor = documento.IdDocPredecesor,
                    Doc_Version = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocumentoRuta = documento.DocRuta,
                    Revisores = JsonConvert.SerializeObject(revisores),
                    Aprobadores = JsonConvert.SerializeObject(aprobadores),
                    Secundarios = JsonConvert.SerializeObject(secundarios)
                };

                ViewBag.Error = Error;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarDocumentoSRevision(SGADocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.Id_SGA_Tipo_Documento));
                var revisores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>("[]");
                var aprobadores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>("[]");

                if (model.Revisores != null)
                    revisores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores);
                if (model.Aprobadores != null)
                    aprobadores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores);

                if (model.Documento != null)
                {
                    var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre + "_v." + String.Format("{0:0.00}", model.Doc_Version) + Path.GetExtension(model.Documento.FileName);
                    var cargaDocumento = await CargarDocumento(model.Documento, nombreDoc, tipoDoc.TipoDocumento.Descripcion);
                    if (!cargaDocumento.Equals("Error"))
                    {
                        await DeleteFTPFileAsync(model.DocumentoRuta);
                        await _editarDocumento.Execute(
                            model.Id_SGA_Documento,
                            model.Id_SGA_Tipo_Documento,
                            model.Doc_Codigo,
                            model.Doc_Nombre,
                            model.Doc_Version,
                            model.Doc_Predecesor,
                            model.Comentario,
                            cargaDocumento,
                            userInfo.NombreUsuario,
                            model.Opcion,
                            revisores,
                            aprobadores);
                        if (model.Opcion.Equals("Registrar") && !model.Opcion.Equals("AgregarFormato"))
                            return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()) });
                        else if (model.Opcion.Equals("AgregarFormato"))
                            return RedirectToAction("CargarDocumentoSRevision", new { IdDocPrincipal = model.Id_SGA_Documento, IdCategoria = model.Id_Categoria });
                        else
                            return RedirectToAction("EditarDocumentoSRevision", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()), IdCategoria = model.Id_Categoria });
                    }
                    return RedirectToAction("EditarDocumentoSRevision", new { Error = "Ocurrió un error al cargar el archivo" });
                }
                else
                {
                    await _editarDocumento.Execute(
                            model.Id_SGA_Documento,
                            model.Id_SGA_Tipo_Documento,
                            model.Doc_Codigo,
                            model.Doc_Nombre,
                            model.Doc_Version,
                            model.Doc_Predecesor,
                            model.Comentario,
                            model.DocumentoRuta,
                            userInfo.NombreUsuario,
                            model.Opcion,
                            revisores,
                            aprobadores);
                    if (model.Opcion.Equals("Registrar"))
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()) });
                    else if (model.Opcion.Equals("AgregarFormato"))
                        return RedirectToAction("CargarDocumentoSRevision", new { IdDocPrincipal = model.Id_SGA_Documento, IdCategoria = model.Id_Categoria });
                    else
                        return RedirectToAction("EditarDocumentoSRevision", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()), IdCategoria = model.Id_Categoria });
                }

            }
            return View(model);
        }

        public async Task<IActionResult> FirmarDocumento(string IdDocumento, int IdCategoria, string Error)
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

                var tipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

                ViewBag.Usuarios = usuarios;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                var documento = await _consultarDocumento.Execute(IdDesencriptado);
                //Obtener documentos que tienen como documento principal el actual
                var documentosSecundarios = await _consultarDocumentos.ExecuteSecundarios(IdDesencriptado);
                var secundarios = documentosSecundarios.Select(x => new
                {
                    IdEncriptado = _protector.Protect(x.IdSGADocumento.ToString()),
                    Codigo = x.DocCodigo,
                    TipoDocumento = x.SGATipoDocumento,
                    Nombre = x.DocNombre,
                    Version = x.DocVersion,
                    Condicion = x.Condicion,
                    Ruta = x.DocRuta
                }).ToList();

                var revisores = documento.Revisores.Select(x => new {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd")
                })
                    .ToList();

                var aprobadores = documento.Aprobadores.Select(x => new {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"),
                })
                    .ToList();

                var model = new SGADocumentoModel
                {
                    Id_Encriptado = IdDocumento,
                    Id_SGA_Documento = documento.IdSGADocumento,
                    Id_Categoria = IdCategoria,
                    Id_SGA_Tipo_Documento = documento.IdSGACategoriaTipo,
                    Doc_Nombre = documento.DocNombre,
                    Doc_Predecesor = documento.IdDocPredecesor,
                    Doc_Codigo = documento.DocCodigo,
                    Doc_Version = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocumentoRuta = documento.DocRuta,
                    Revisores = JsonConvert.SerializeObject(revisores),
                    Aprobadores = JsonConvert.SerializeObject(aprobadores),
                    Secundarios = JsonConvert.SerializeObject(secundarios)
                };

                ViewBag.Error = Error;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> FirmarDocumento(SGADocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.Id_SGA_Tipo_Documento));
                var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre + "_v." + String.Format("{0:0.00}", model.Doc_Version) + Path.GetExtension(model.Documento.FileName);
                var cargaDocumento = await CargarDocumento(model.Documento, nombreDoc, tipoDoc.TipoDocumento.Descripcion);
                if (!cargaDocumento.Equals("Error"))
                {
                        await _firmarDocumento.Execute(
                            model.Id_SGA_Documento,
                            cargaDocumento,
                            tipoDoc.TipoDocumento.Codigo,
                            tipoDoc.IdSGACategoriaTipo);
                    return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.Id_SGA_Documento.ToString()) });
                }
                return RedirectToAction("FirmarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
            }
            return View(model);
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
                    filasTabla += "<td><a href=\"/SGAAdministracion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
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
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"}
            };
        }
        private async Task<string> CargarDocumento(IFormFile file, string newFileName, string tipo)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre-Prueba/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);

                var fileName = filePath + newFileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    file.CopyTo(stream);

                    stream.Flush();

                    stream.Close();
                }
                var newFilePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/" + newFileName;
                //await EncryptFileAsync(fileName, encryptedArchiveFile);

                return await UploadFTPFileAsync(newFilePath, newFileName, tipo);
            }
            else
                return "Error";
        }
        public async Task<string> UploadFTPFileAsync(string path, string newFileName, string tipo)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                try
                {
                    var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionSGADocumentos);
                    string host = "", port = "", user = "", pass = "", dir = "";
                    foreach (var item in serverInfo.Info)
                    {
                        switch (item.Key)
                        {
                            case "host": host = item.Value; break;
                            case "port": port = item.Value; break;
                            case "user": user = item.Value; break;
                            case "password": pass = item.Value; break;
                            case "directory": dir = item.Value; break;
                        }
                    }

                    using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                    {
                        sftp.Connect();
                        if (!sftp.Exists(Path.Combine(dir, tipo)))
                            sftp.CreateDirectory(Path.Combine(dir, tipo));

                        sftp.ChangeDirectory(Path.Combine(dir, tipo));
                        using (FileStream fs = System.IO.File.OpenRead(path))
                        {
                            sftp.UploadFile(fs, newFileName, true);
                        }
                        sftp.Dispose();
                    }
                    return Path.Combine(tipo, newFileName);
                }
                catch (Exception e)
                {
                    return "Error";
                }
            }
            else
                return "Error";
        }
        private async Task DeleteFTPFileAsync(string filename)
        {
            var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionSGADocumentos);
            string host = "", port = "", user = "", pass = "", dir = "";
            foreach (var item in  serverInfo.Info)
            {
                switch (item.Key)
                {
                    case "host": host = item.Value; break;
                    case "port": port = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                    case "directory": dir = item.Value; break;
                }
            }
            try
            {
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);

                    sftp.DeleteFile(filename);

                    sftp.Dispose();
                }
            }catch(Exception e) { }
        }
    }
}
