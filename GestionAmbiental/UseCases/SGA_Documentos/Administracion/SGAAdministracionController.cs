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
using GestionAmbiental.Application.UseCases.SGA_Documentos.DeleteDocument;
using System.IO;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using Renci.SshNet;
using GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.FirmarDocumento;
using Microsoft.AspNetCore.Hosting;
using GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarFormatoDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.AmpliarFechaRevision;
using Serilog;

namespace GestionAmbiental.UseCases.SGA_Documentos.Administracion
{
    public class SGAAdministracionController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarFormatoDocumento _registrarFormatoDocumento;
        private readonly IConsultarDocumentos _consultarDocumentos;
        private readonly IConsultarDocumento _consultarDocumento;
        private readonly IConsultarPredecesores _consultarPredecesores;
        private readonly IConsultarRevisionDocumento _consultarRevisionDocumento;
        private readonly IGetUsuario _getUsuario;
        private readonly IEditarDocumento _editarDocumento;
        private readonly IRegistrarDocumento _registrarDocumento;
        private readonly IFirmarDocumento _firmarDocumento;
        private readonly IAmpliarFechaRevision _ampliarFechaRevision;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly IDeleteDocument _deleteDocument;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        public SGAAdministracionController(
                    ISelectItems selectItems,
                    IRegistrarFormatoDocumento registrarFormatoDocumento,
                    IConsultarDocumentos consultarDocumentos,
                    IConsultarDocumento consultarDocumento,
                    IConsultarPredecesores consultarPredecesores,
                    IConsultarRevisionDocumento consultarRevisionDocumento,
                    IGetUsuario getUsuario,
                    IEditarDocumento editarDocumento,
                    IGetInfo_SGA info_SGA,
                    IRegistrarDocumento registrarDocumento,
                    IAmpliarFechaRevision ampliarFechaRevision,
                    IFirmarDocumento firmarDocumento,
                    IWebHostEnvironment hostingEnvironment,
                    IDataProtectionProvider dataProtectionProvider,
                    IDeleteDocument deleteDocument,
                    DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _selectItems = selectItems;
            _registrarFormatoDocumento = registrarFormatoDocumento;
            _consultarDocumentos = consultarDocumentos;
            _consultarPredecesores = consultarPredecesores;
            _consultarDocumento = consultarDocumento;
            _consultarRevisionDocumento = consultarRevisionDocumento;
            _getUsuario = getUsuario;
            _info_SGA = info_SGA;
            _registrarDocumento = registrarDocumento;
            _firmarDocumento = firmarDocumento;
            _ampliarFechaRevision = ampliarFechaRevision;
            _hostingEnvironment = hostingEnvironment;
            _editarDocumento = editarDocumento;
            _deleteDocument = deleteDocument;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.DocumentosSGAIdRouteValue);
        }
        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
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
                    if (!fila.Estado.Equals(EstadosDocumentosSGA.Pendiente.ToString()))
                        filasTabla += "<td><a href=\"/SGAAdministracion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a>";
                    else
                    {
                        if (!fila.Principal && !fila.IdSGACategoriaDocumento.Equals(1))
                            filasTabla += "<td><a href=\"/SGAAdministracion/EditarDocumentoSRevision?IdDocumento=" + id_encriptado + "&IdCategoria=" + fila.IdSGACategoriaDocumento + "\">" + fila.DocCodigo + "</a>";
                        else
                            filasTabla += "<td><a href=\"/SGAAdministracion/EditarDocumento?IdDocumento=" + id_encriptado + "&IdCategoria=" + fila.IdSGACategoriaDocumento + "\">" + fila.DocCodigo + "</a>";
                    }
                    //filasTabla += "<button class='btn btn-mc form-control' type=\"button\" onclick=\"this.disabled=true;PreviewPDF(\'" + fila.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">Descargar Pdf</button></td>";

                    filasTabla += " &nbsp; &nbsp;<a style=\"cursor:pointer\" onclick=\"this.disabled=true;PreviewPDF(\'" + fila.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">" +
                        "<svg xmlns=\"http://www.w3.org/2000/svg\"  viewBox=\"0 0 48 48\" width=\"24px\" height=\"24px\"><path fill=\"#616161\" d=\"M34.6 28.1H38.6V45.1H34.6z\" transform=\"rotate(-45.001 36.586 36.587)\"/><path fill=\"#616161\" d=\"M20 4A16 16 0 1 0 20 36A16 16 0 1 0 20 4Z\"/><path fill=\"#37474F\" d=\"M36.2 32.1H40.2V44.400000000000006H36.2z\" transform=\"rotate(-45.001 38.24 38.24)\"/><path fill=\"#64B5F6\" d=\"M20 7A13 13 0 1 0 20 33A13 13 0 1 0 20 7Z\"/><path fill=\"#BBDEFB\" d=\"M26.9,14.2c-1.7-2-4.2-3.2-6.9-3.2s-5.2,1.2-6.9,3.2c-0.4,0.4-0.3,1.1,0.1,1.4c0.4,0.4,1.1,0.3,1.4-0.1C16,13.9,17.9,13,20,13s4,0.9,5.4,2.5c0.2,0.2,0.5,0.4,0.8,0.4c0.2,0,0.5-0.1,0.6-0.2C27.2,15.3,27.2,14.6,26.9,14.2z\"/></svg>" +
                        "</a></td>";
                    filasTabla += "<td>" + fila.DocNombre + "</td>";
                    filasTabla += "<td>" + fila.DocVersion + "</td>";
                    filasTabla += "<td>";
                    filasTabla += fila.FechaAprobacion.HasValue ? fila.FechaAprobacion.Value.ToString("dd/MM/yyyy") : "";
                    filasTabla += "</td>";
                    filasTabla += "<td>" + fila.Condicion + "</td>";
                    filasTabla += "<td>" + fila.Comentario + "</td>";
                    filasTabla += "<td>";
                    if (categoria != 3)
                        filasTabla += fila.FechaTentativa.HasValue ? fila.FechaTentativa.Value.ToString("dd/MM/yyyy") : "";
                    filasTabla += "</td>";

                    filasTabla += "<td><a style=\"cursor:pointer\" onclick=\" deleteDocument(\'" + fila.IdSGADocumento + "\') \">" +
                        "<svg xmlns=\"http://www.w3.org/2000/svg\" id=\"Outline\" viewBox=\"0 0 24 24\" width=\"20px\" height=\"20px\"><path d=\"M21,4H17.9A5.009,5.009,0,0,0,13,0H11A5.009,5.009,0,0,0,6.1,4H3A1,1,0,0,0,3,6H4V19a5.006,5.006,0,0,0,5,5h6a5.006,5.006,0,0,0,5-5V6h1a1,1,0,0,0,0-2ZM11,2h2a3.006,3.006,0,0,1,2.829,2H8.171A3.006,3.006,0,0,1,11,2Zm7,17a3,3,0,0,1-3,3H9a3,3,0,0,1-3-3V6H18Z\"/><path d=\"M10,18a1,1,0,0,0,1-1V11a1,1,0,0,0-2,0v6A1,1,0,0,0,10,18Z\"/><path d=\"M14,18a1,1,0,0,0,1-1V11a1,1,0,0,0-2,0v6A1,1,0,0,0,14,18Z\"/></svg>" +
                        "</a></td>";


                    filasTabla += "</tr>";
                }
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";
            }
            else
            {
                filasTabla = "<tr><td colspan=\"100%\">Sin datos</td></tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetTablaModalResumenFiltrada(int categoria, int[] tipos, int[] condiciones, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute("", categoria, tipos, "", condiciones, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Documentos)
                {
                    var id_encriptado = _protector.Protect(fila.IdSGADocumento.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.DocCodigo + "</td>";
                    filasTabla += "<td>" + fila.DocNombre + "</td>";
                    filasTabla += "<td>" + fila.DocVersion + "</td>";
                    filasTabla += "<td>";
                    filasTabla += fila.FechaAprobacion.HasValue ? fila.FechaAprobacion.Value.ToString("dd/MM/yyyy") : "";
                    filasTabla += "</td>";
                    filasTabla += "<td>" + fila.Condicion + "</td>";
                    filasTabla += "<td>" + fila.Comentario + "</td>";
                    filasTabla += "<td>";
                    filasTabla += fila.FechaTentativa.HasValue ? fila.FechaTentativa.Value.ToString("dd/MM/yyyy") : "";
                    filasTabla += "</td>";
                    if (fila.PdfRuta != null)
                        filasTabla += "<td><button type =\"button\" class=\"btn btn-mc\" onclick=\"this.disabled=true;PreviewPDF(\'" + fila.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">Visualizar Pdf</button></td>";
                    else
                        filasTabla += "<td></td>";

                }
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrarModal(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrarModal(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                filasTabla = "<tr><td colspan=\"100%\">Sin datos</td></tr>";
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdDocumento=" + IdDocumento });
            }
            else
            {
                var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                var usuarioActual = await _getUsuario.Execute(userInfo.NombreUsuario);

                var documento = await _consultarDocumento.Execute(IdDesencriptado);

                var revisores = documento.Revisores.Select(x => new
                {
                    Id = _protector.Protect(x.IdSGARevisionDocumento.ToString()),
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaRevision.HasValue ? x.FechaRevision.Value.ToString("yyyy-MM-dd") : "",
                    Estado = x.Estado,
                    //El creador del documento es el usuario conectado?
                    Permiso = documento.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false
                })
                    .ToList();

                var aprobadores = documento.Aprobadores.Select(x => new
                {
                    Id = _protector.Protect(x.IdSGARevisionDocumento.ToString()),
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaRevision.HasValue ? x.FechaRevision.Value.ToString("yyyy-MM-dd") : "",
                    Estado = x.Estado,
                    //El creador del documento es el usuario conectado?
                    Permiso = documento.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false
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
                    PdfRuta = x.PdfRuta,
                    EditableRuta = x.EditableRuta
                }).ToList();

                var usuarios = items.ItemsUsuarios.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Usuarios = usuarios;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = categoriasTipos;
                ViewBag.CategoriaDoc = documento.SGACategoriaDocumento;

                var model = new SGADocumentoModel
                {
                    IdEncriptado = IdDocumento,
                    IdSGADocumento = documento.IdSGADocumento,
                    IdSGATipoDocumento = documento.IdSGACategoriaTipo,
                    IdElaboradoPor = documento.IdUsuario,
                    DocNombre = documento.DocNombre,
                    DocPredecesor = documento.IdDocPredecesor,
                    DocCodigo = documento.DocCodigo,
                    DocVersion = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocPdfRuta = documento.PdfRuta,
                    DocEditableRuta = documento.EditableRuta,
                    FechaTentativa = documento.FechaTentativa != null ? documento.FechaTentativa.Value.ToString("dd/MM/yyyy") : "",
                    FechaAprobacion = documento.FechaAprobacion != null ? documento.FechaAprobacion.Value.ToString("dd/MM/yyyy") : "",
                    Revisores = JsonConvert.SerializeObject(revisores),
                    Aprobadores = JsonConvert.SerializeObject(aprobadores),
                    Secundarios = JsonConvert.SerializeObject(secundarios)
                };
                return View(model);
            }
        }
        //Solo Documentos SGA
        public async Task<IActionResult> ConsultarPublicados(int IdCategoria)
        {
            dynamic response;
            try
            {
                var publicadosData = await _consultarDocumentos.ExecutePublicados();
                var documentos = publicadosData.Where(x => x.IdSGACategoriaDocumento.Equals(IdCategoria)).ToList();
                var publicados = documentos.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString())).Select(x => new
                {
                    Id = x.IdSGADocumento,
                    Text = "(" + x.DocCodigo + ") " + x.DocNombre + " v." + x.DocVersion.ToString(),
                    Editable = "<button type=\"button\" class=\"btn btn-primary\" onclick=\"DescargarEditable(\'" + x.EditableRuta.Replace("\\", "-") + "\')\">Descargar Editable </button>",
                    Html = "<tr iddoc=\"" + x.IdSGADocumento + "\"><td><a href=\"/SGAAdministracion/" + (x.Condicion == EstadosDocumentosSGA.Pendiente.ToString() ? "EditarDocumento" : "VerDocumento") + "?IdDocumento=" + _protector.Protect(x.IdSGADocumento.ToString()) + "&IdCategoria=" + IdCategoria + "\">" + x.DocCodigo + "</a></td>" +
                    "<td>" + x.SGATipoDocumento + "</td>" +
                    "<td>" + x.DocNombre + "</td>" +
                    "<td>" + x.DocVersion + "</td>" +
                    "<td>" + x.Condicion + "</td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"this.disabled=true;PreviewPDF(\'" + x.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">Visualizar Pdf</button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"DescargarEditable(\'" + x.EditableRuta.Replace("\\", "-") + "\')\">Descargar Editable </button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarSecundario(" + x.IdSGADocumento + ")\">Eliminar</button></td>" +
                    "</tr>"
                });
                response = new
                {
                    success = true,
                    publicadosHtml = JsonConvert.SerializeObject(publicados),
                    listDocumentosP = JsonConvert.SerializeObject(documentos),
                    publicadosEditable = JsonConvert.SerializeObject(JsonConvert.SerializeObject(publicados.Select(g => new { Id = g.Id.ToString(), Button = g.Editable }).ToList()))
                };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }
        public async Task<IActionResult> CargarDocumento(string Error, int IdCategoria)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Error=" + Error + "&IdCategoria=" + IdCategoria });
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
                    case 2: tituloCategoria = "SIG"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }
                ViewBag.CategoriaDoc = tituloCategoria;

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var categoriastipo = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).ToList();
                var categoriastipos = categoriastipo.Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriastipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var publicadosData = await _consultarDocumentos.ExecutePublicados();
                var publicadosCodigo = publicadosData.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && x.IdSGACategoriaDocumento.Equals(IdCategoria)).Select(x => x.DocCodigo).ToList();
                var publicadoss = publicadosData.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && x.IdSGACategoriaDocumento.Equals(IdCategoria)).Select(x => new
                {
                    Id = x.IdSGADocumento,
                    Text = "(" + x.DocCodigo + ") " + x.DocNombre + " v." + x.DocVersion.ToString(),
                    Html = "<tr iddoc=\"" + x.IdSGADocumento + "\"><td><a href=\"/SGAAdministracion/" + (x.Condicion == EstadosDocumentosSGA.Pendiente.ToString() ? "EditarDocumento" : "VerDocumento") + "?IdDocumento=" + _protector.Protect(x.IdSGADocumento.ToString()) + "&IdCategoria=" + IdCategoria + "\">" + x.DocCodigo + "</a></td>" +
                    "<td>" + x.SGATipoDocumento + "</td>" +
                    "<td>" + x.DocNombre + "</td>" +
                    "<td>" + x.DocVersion + "</td>" +
                    "<td>" + x.Condicion + "</td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"this.disabled=true;PreviewPDF(\'" + x.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">Visualizar Pdf</button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"DescargarEditable(\'" + x.EditableRuta.Replace("\\", "-") + "\')\">Descargar Editable </button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarSecundario(" + x.IdSGADocumento + ")\">Eliminar</button></td>" +
                    "</tr>"
                });
                var listPublicados = publicadoss.Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Text }).ToList();
                listPublicados.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                ViewBag.IdCategoria = IdCategoria;
                ViewBag.PublicadosHtml = JsonConvert.SerializeObject(publicadoss);
                ViewBag.PublicadosCodigo = JsonConvert.SerializeObject(publicadosCodigo);
                ViewBag.Publicados = listPublicados;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = categoriastipos;
                ViewBag.TiposDocRuta = JsonConvert.SerializeObject(categoriastipo.Select(x => new { Id = x.IdSGACategoriaTipo, Descripcion = x.TipoDocumento.Descripcion, Ruta = x.TipoDocumento.Formato != null ? 1 : 0 }));
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
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.IdSGATipoDocumento));
                var nombreDocPdf = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoPdf.FileName);
                var nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
                var revisores = model.Revisores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores) : new List<RevisionAprobacionInputOutput>();
                var aprobadores = model.Aprobadores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores) : new List<RevisionAprobacionInputOutput>();
                var secundarios = model.Secundarios != null ? JsonConvert.DeserializeObject<List<int>>(model.Secundarios) : new List<int>();
                var docPdfRuta = await CargarDocumento(model.DocumentoPdf, nombreDocPdf, tipoDoc.TipoDocumento.Descripcion);
                var docEditableRuta = await CargarDocumento(model.DocumentoEditable, nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);

                if (!docPdfRuta.Equals("Error") && !docEditableRuta.Equals("Error"))
                {
                    var idDocumento = await _registrarDocumento.Execute(
                        model.IdSGATipoDocumento,
                        model.DocCodigo,
                        model.DocPrincipal,
                        tipoDoc.TipoDocumento.Codigo,
                        model.DocNombre,
                        model.DocVersion,
                        model.DocPredecesor,
                        model.Comentario,
                        docPdfRuta,
                        docEditableRuta,
                        model.FechaTentativa,
                        model.FechaAprobacion,
                        model.IdElaboradoPor,
                        model.Opcion,
                        revisores,
                        aprobadores,
                        secundarios);
                    if (model.Opcion.Equals("GuardarEstado"))
                        //Falta IdCategoria
                        return RedirectToAction("EditarDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                    else
                        //Registrar
                        return RedirectToAction("NotificarNuevoDocumento", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(idDocumento.ToString()) });
                    //return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento});
                }
                else
                {
                    return RedirectToAction("CargarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
                }
            }
            return View(model);
        }
        //Documentos SIG y externos. Documentos secundarios (Formatos)
        public async Task<IActionResult> CargarDocumentoSRevision(string Error, int IdCategoria)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Error=" + Error + "&IdCategoria=" + IdCategoria });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var categoriastipo = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).ToList();
                var categoriastipos = categoriastipo.Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriastipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SIG"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }
                ViewBag.CategoriaDoc = tituloCategoria;

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.IdCategoria = IdCategoria;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = categoriastipos;
                ViewBag.TiposDocRuta = JsonConvert.SerializeObject(categoriastipo.Select(x => new { Id = x.IdSGACategoriaTipo, Descripcion = x.TipoDocumento.Descripcion, Ruta = x.TipoDocumento.Formato != null ? 1 : 0 }));
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
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.IdSGATipoDocumento));
                //if (model.IdCategoria == 0)
                //    model.IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento;
                var secundarios = model.Secundarios != null ? JsonConvert.DeserializeObject<List<int>>(model.Secundarios) : new List<int>();
                var nombreDocPdf = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoPdf.FileName);
                var nombreDocEditable = "";
                if (model.DocumentoEditable != null)
                {
                    nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
                }
                var cargaDocumentoPdf = await CargarDocumento(model.DocumentoPdf, nombreDocPdf, tipoDoc.TipoDocumento.Descripcion);
                var cargaDocumentoEditable = "";
                if (model.DocumentoEditable != null)
                {
                    cargaDocumentoEditable = await CargarDocumento(model.DocumentoEditable, nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);
                }
                var revisores = model.Revisores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores) : new List<RevisionAprobacionInputOutput>();
                var aprobadores = model.Aprobadores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores) : new List<RevisionAprobacionInputOutput>();
                if (model.IdElaboradoPor == 0)
                {
                    var dataUsuario = await _getUsuario.Execute(userInfo.NombreUsuario);
                    model.IdElaboradoPor = dataUsuario.IdUsuario;
                }
                //if (!cargaDocumentoPdf.Equals("Error") && !cargaDocumentoEditable.Equals("Error"))

                if (!cargaDocumentoPdf.Equals("Error"))
                {
                    var idDocumento = await _registrarDocumento.Execute(
                        model.IdSGATipoDocumento,
                        model.DocCodigo,
                        model.DocPrincipal,
                        tipoDoc.TipoDocumento.Codigo,
                        model.DocNombre,
                        model.DocVersion,
                        model.DocPredecesor,
                        model.Comentario,
                        cargaDocumentoPdf,
                        cargaDocumentoEditable,
                        model.FechaTentativa,
                        model.FechaAprobacion,
                        model.IdElaboradoPor,
                        model.Opcion,
                        revisores,
                        aprobadores,
                        secundarios
                        );
                    if (model.Opcion.Equals("GuardarEstado"))
                        return RedirectToAction("EditarDocumentoSRevision", new { IdDocumento = _protector.Protect(idDocumento.ToString()), IdCategoria = model.IdCategoria });
                    else
                        //return RedirectToAction("NotificarNuevoDocumento", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(idDocumento.ToString()) });
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()) });
                }
                else
                {
                    return RedirectToAction("CargarDocumentoSRevision", new { Error = "Ocurrió un error al cargar el archivo" });
                }
            }
            return View(model);
        }
        [HttpPost]
        //public async Task<IActionResult> CargarFormato(int Id_Tipo_Documento_Formato, int Id_Categoria_Formato, string Doc_Nombre_Formato, double Doc_Version_Formato, string Comentario_Formato, IList<IFormFile> Documento_Formato)
        public async Task<IActionResult> CargarFormato(FormatoDocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.Id_Tipo_Documento_Formato));
                var nombreDocPdf = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre_Formato + "_v." + String.Format("{0:0.00}", model.Doc_Version_Formato) + Path.GetExtension(model.Documento_Formato_Pdf[0].FileName);
                var cargaDocumentoPdf = await CargarDocumento(model.Documento_Formato_Pdf[0], nombreDocPdf, tipoDoc.TipoDocumento.Descripcion);
                var nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre_Formato + "_v." + String.Format("{0:0.00}", model.Doc_Version_Formato) + Path.GetExtension(model.Documento_Formato_Editable[0].FileName);
                var cargaDocumentoEditable = await CargarDocumento(model.Documento_Formato_Editable[0], nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);

                dynamic response;
                if (!cargaDocumentoPdf.Equals("Error") && !cargaDocumentoEditable.Equals("Error"))
                {
                    var idDocumento = await _registrarFormatoDocumento.Execute(
                      model.Id_Tipo_Documento_Formato,
                      model.Doc_Nombre_Formato,
                      model.Doc_Version_Formato,
                      model.Comentario_Formato,
                      model.Fecha_Tentativa,
                      model.Fecha_Aprobacion,
                      cargaDocumentoPdf,
                      cargaDocumentoEditable,
                      model.Id_Elaborado_Por
                      );
                    var nuevoDoc = await _consultarDocumento.Execute(idDocumento);
                    var id_encriptado = _protector.Protect(nuevoDoc.IdSGADocumento.ToString());
                    var html = "<tr>";
                    html += "<tr iddoc=\"" + nuevoDoc.IdSGADocumento + "\"><td><a href=\"/SGAAdministracion/" + (nuevoDoc.Condicion == EstadosDocumentosSGA.Pendiente.ToString() ? "EditarDocumento" : "VerDocumento") + "?IdDocumento=" + _protector.Protect(nuevoDoc.IdSGADocumento.ToString()) + "&IdCategoria=" + tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento + "\">" + nuevoDoc.DocCodigo + "</a></td>";
                    html += "<td>" + nuevoDoc.SGATipoDocumento + "</td>" +
                        "<td>" + nuevoDoc.DocNombre + "</td>" +
                        "<td>" + nuevoDoc.DocVersion + "</td >" +
                        "<td>" + nuevoDoc.Condicion + "</td >" +
                        "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"this.disabled=true;PreviewPDF(\'" + nuevoDoc.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">Visualizar</button></td >" +
                        "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"DescargarEditable(\'" + nuevoDoc.EditableRuta.Replace('\\', '-') + "\')\">Descargar Editable</button></td >" +
                        "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarSecundario(" + nuevoDoc.IdSGADocumento + ")\">Eliminar</button></td>" +
                        "</tr>";
                    response = new
                    {
                        success = true,
                        idDoc = nuevoDoc.IdSGADocumento,
                        html = html
                    };
                    return Ok(response);
                }
                else
                {
                    response = new
                    {
                        success = false
                    };
                    return Ok(response);
                }
            }
            return View();
        }
        //Solo Documentos SGA
        public async Task<IActionResult> ActualizarDocumento(string Error, int IdCategoria)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Error=" + Error + "&IdCategoria=" + IdCategoria });
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
                var categoriastipo = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).ToList();
                var categoriastipos = categoriastipo.Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriastipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var categoriasTiposExistentes = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria) && tiposDocExistentes.Contains(x.IdSGACategoriaTipo)).OrderBy(x => x.IdSGACategoriaTipo)/*.OrderBy(y => y.TipoDocumento.Descripcion)*/.Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriasTiposExistentes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var documentosPublicados = JsonConvert.SerializeObject(documentos);

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SIG"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }

                ViewBag.CategoriaDoc = tituloCategoria;
                ViewBag.Error = Error;

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

                var publicados = documentos.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && x.IdSGACategoriaDocumento.Equals(IdCategoria)).Select(x => new
                {
                    Id = x.IdSGADocumento,
                    Text = "(" + x.DocCodigo + ") " + x.DocNombre + " v." + x.DocVersion.ToString(),
                    Editable = "<button type=\"button\" class=\"btn btn-primary\" onclick=\"DescargarEditable(\'" + x.EditableRuta.Replace("\\", "-") + "\')\">Descargar Editable </button>",
                    Html = "<tr iddoc=\"" + x.IdSGADocumento + "\"><td><a href=\"/SGAAdministracion/" + (x.Condicion == EstadosDocumentosSGA.Pendiente.ToString() ? "EditarDocumento" : "VerDocumento") + "?IdDocumento=" + _protector.Protect(x.IdSGADocumento.ToString()) + "&IdCategoria=" + IdCategoria + "\">" + x.DocCodigo + "</a></td>" +
                    "<td>" + x.SGATipoDocumento + "</td>" +
                    "<td>" + x.DocNombre + "</td>" +
                    "<td>" + x.DocVersion + "</td>" +
                    "<td>" + x.Condicion + "</td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"this.disabled=true;PreviewPDF(\'" + x.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">Visualizar Pdf</button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"DescargarEditable(\'" + x.EditableRuta.Replace("\\", "-") + "\')\">Descargar Editable </button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarSecundario(" + x.IdSGADocumento + ")\">Eliminar</button></td>" +
                    "</tr>"
                });
                var listPublicados = publicados.Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Text }).ToList();
                listPublicados.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.IdCategoria = IdCategoria;
                ViewBag.DocumentosPublicados = documentosPublicados;
                ViewBag.Publicados = listPublicados;
                ViewBag.PublicadosEditable = JsonConvert.SerializeObject(publicados.Select(g => new { Id = g.Id.ToString(), Button = g.Editable }).ToList());
                ViewBag.PublicadosHtml = JsonConvert.SerializeObject(publicados);
                ViewBag.CategoriaDoc = tituloCategoria;
                ViewBag.Error = Error;
                ViewBag.TiposDocExistentes = categoriasTiposExistentes;
                ViewBag.TiposDoc = categoriastipos;
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
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.IdSGATipoDocumento));
                var nombreDocPdf = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoPdf.FileName);
                var nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
                var revisores = model.Revisores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores) : new List<RevisionAprobacionInputOutput>();
                var aprobadores = model.Aprobadores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores) : new List<RevisionAprobacionInputOutput>();
                var secundarios = model.Secundarios != null ? JsonConvert.DeserializeObject<List<int>>(model.Secundarios) : new List<int>();
                var docPdfRuta = await CargarDocumento(model.DocumentoPdf, nombreDocPdf, tipoDoc.TipoDocumento.Descripcion);
                var docEditableRuta = await CargarDocumento(model.DocumentoEditable, nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);
                if (!docPdfRuta.Equals("Error") && !docEditableRuta.Equals("Error"))
                {
                    var idDocumento = await _registrarDocumento.Execute(
                        model.IdSGATipoDocumento,
                        model.DocCodigo,
                        model.DocPrincipal,
                        tipoDoc.TipoDocumento.Codigo,
                        model.DocNombre,
                        model.DocVersion,
                        model.DocPredecesor,
                        model.Comentario,
                        docPdfRuta,
                        docEditableRuta,
                        model.FechaTentativa,
                        model.FechaAprobacion,
                        model.IdElaboradoPor,
                        model.Opcion,
                        revisores,
                        aprobadores,
                        secundarios);
                    if (model.Opcion.Equals("GuardarEstado"))
                        //Falta IdCategoria
                        return RedirectToAction("EditarDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                    else
                        //Registrar
                        return RedirectToAction("NotificarNuevoDocumento", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(idDocumento.ToString()) });
                    //return RedirectToAction("NotificarRevisionActiva", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(idDocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                    //return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(idDocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                }
                else
                {
                    return RedirectToAction("ActualizarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
                }
            }
            return View(model);
        }
        //Documentos SIG y externos.
        public async Task<IActionResult> ActualizarDocumentoSRevision(string Error, int IdCategoria, int IdDocPrincipal)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Error=" + Error + "&IdCategoria=" + IdCategoria + "&IdDocPrincipal=" + IdDocPrincipal });
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


                var publicados = documentos.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && x.IdSGACategoriaDocumento.Equals(IdCategoria)).Select(x => new
                {
                    Id = x.IdSGADocumento,
                    Editable = ""
                });

                if (IdCategoria != 3)
                {
                    publicados = documentos.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && x.IdSGACategoriaDocumento.Equals(IdCategoria)).Select(x => new
                    {
                        Id = x.IdSGADocumento,
                        Editable = "<button type=\"button\" class=\"btn btn-mc\" onclick=\"DescargarEditable(\'" + x.EditableRuta.Replace("\\", "-") + "\')\">Descargar Editable </button>"
                    });
                }

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SIG"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }

                ViewBag.PublicadosEditable = JsonConvert.SerializeObject(publicados.Select(g => new { Id = g.Id.ToString(), Button = g.Editable }).ToList());
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Error=" + Error + "&IdCategoria=" + IdCategoria + "&IdDocumento=" + IdDocumento });
            }
            else
            {
                var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SIG"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }
                ViewBag.CategoriaDoc = tituloCategoria;
                var usuarioActual = await _getUsuario.Execute(userInfo.NombreUsuario);

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Usuarios = usuarios;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                var documento = await _consultarDocumento.Execute(IdDesencriptado);
                //Obtener documentos que tienen como documento principal el actual
                var documentosSecundarios = await _consultarDocumentos.ExecuteSecundarios(IdDesencriptado);
                var secundarios = documentosSecundarios.Select(x => new SecundariosInput
                {
                    IdEncriptado = _protector.Protect(x.IdSGADocumento.ToString()),
                    Id = x.IdSGADocumento,
                    Codigo = x.DocCodigo,
                    TipoDocumento = x.SGATipoDocumento,
                    Nombre = x.DocNombre,
                    Version = x.DocVersion,
                    Condicion = x.Condicion,
                    RutaPdf = x.PdfRuta,
                    RutaEditable = x.EditableRuta
                }).ToList();
                var publicadosData = await _consultarDocumentos.ExecutePublicados();
                var publicadosCodigo = publicadosData.Where(x => x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) && x.IdSGACategoriaDocumento.Equals(IdCategoria)).Select(x => x.DocCodigo).ToList();
                var publicados = publicadosData.Where(x =>
                x.Estado.Equals(EstadosDocumentosSGA.Aprobado.ToString()) &&
                 x.IdSGACategoriaDocumento.Equals(IdCategoria) &&
                !secundarios.Select(y => y.Id).Contains(x.IdSGADocumento))
                .Select(x => new
                {
                    Id = x.IdSGADocumento,
                    Text = "(" + x.DocCodigo + ") " + x.DocNombre + " v." + x.DocVersion.ToString(),
                    Html = "<tr iddoc=\"" + x.IdSGADocumento + "\"><td><a href=\"/SGAAdministracion/" + (x.Condicion == EstadosDocumentosSGA.Pendiente.ToString() ? "EditarDocumento" : "VerDocumento") + "?IdDocumento=" + _protector.Protect(x.IdSGADocumento.ToString()) + "&IdCategoria=" + IdCategoria + "\">" + x.DocCodigo + "</a></td>" +
                    "<td>" + x.SGATipoDocumento + "</td>" +
                    "<td>" + x.DocNombre + "</td>" +
                    "<td>" + x.DocVersion + "</td>" +
                    "<td>" + x.Condicion + "</td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"this.disabled=true;PreviewPDF(\'" + x.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">Visualizar Pdf</button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"DescargarEditable(\'" + x.EditableRuta.Replace("\\", "-") + "\')\">Descargar Editable</button></td>" +
                    "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarSecundario(" + x.IdSGADocumento + ")\">Eliminar</button></td>" +
                    "</tr>"
                });
                var listPublicados = publicados.Select(g => new SelectListItem() { Value = g.Id.ToString(), Text = g.Text }).ToList();
                listPublicados.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.PublicadosCodigo = JsonConvert.SerializeObject(publicadosCodigo);
                ViewBag.Publicados = listPublicados;
                ViewBag.PublicadosHtml = JsonConvert.SerializeObject(publicados);
                ViewBag.SecundariosData = JsonConvert.SerializeObject(secundarios);
                var revisores = documento.Revisores.Select(x => new RevisoresAprobadoresInput
                {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaRevision.HasValue ? x.FechaRevision.Value.ToString("yyyy-MM-dd") : ""
                }).OrderBy(x => x.Orden)
                        .ToList();
                var aprobadores = documento.Aprobadores.Select(x => new RevisoresAprobadoresInput
                {
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaRevision.HasValue ? x.FechaRevision.Value.ToString("yyyy-MM-dd") : "",
                }).OrderBy(x => x.Orden)
                    .ToList();

                var model = new SGADocumentoModel
                {
                    IdEncriptado = IdDocumento,
                    IdSGADocumento = documento.IdSGADocumento,
                    IdCategoria = IdCategoria,
                    IdSGATipoDocumento = documento.IdSGACategoriaTipo,
                    IdElaboradoPor = documento.IdUsuario,
                    DocNombre = documento.DocNombre,
                    DocPredecesor = documento.IdDocPredecesor,
                    DocCodigo = documento.DocCodigo,
                    DocVersion = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocPdfRuta = documento.PdfRuta,
                    DocEditableRuta = documento.EditableRuta,
                    FechaTentativa = documento.FechaTentativa != null ? documento.FechaTentativa.Value.ToString("yyyy-MM-dd") : "",
                    FechaAprobacion = documento.FechaAprobacion != null ? documento.FechaAprobacion.Value.ToString("yyyy-MM-dd") : "",
                    Revisores = JsonConvert.SerializeObject(revisores),
                    Aprobadores = JsonConvert.SerializeObject(aprobadores),
                    Secundarios = JsonConvert.SerializeObject(secundarios.Select(x => x.Id).ToList())
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
                var revisores = model.Revisores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores) : new List<RevisionAprobacionInputOutput>();
                var aprobadores = model.Aprobadores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores) : new List<RevisionAprobacionInputOutput>();
                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.IdSGATipoDocumento));
                var secundarios = JsonConvert.DeserializeObject<List<int>>("[]");
                if (model.Secundarios != null)
                    secundarios = JsonConvert.DeserializeObject<List<int>>(model.Secundarios);
                if (model.DocumentoPdf != null || model.DocumentoEditable != null)
                {
                    if (model.DocumentoPdf != null)
                    {
                        var nombreDocPdf = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoPdf.FileName);
                        var cargaDocumentoPdf = await CargarDocumento(model.DocumentoPdf, nombreDocPdf, tipoDoc.TipoDocumento.Descripcion);
                        if (model.DocumentoEditable != null)
                        {
                            var nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
                            var cargaDocumentoEditable = await CargarDocumento(model.DocumentoEditable, nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);
                            await _editarDocumento.Execute(
                               model.IdSGADocumento,
                               tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                               model.IdSGATipoDocumento,
                               model.DocCodigo,
                               tipoDoc.TipoDocumento.Codigo,
                               model.DocNombre,
                               model.DocVersion,
                               model.DocPredecesor,
                               model.Comentario,
                               cargaDocumentoPdf,
                               cargaDocumentoEditable,
                               model.FechaTentativa,
                               model.FechaAprobacion,
                               model.IdElaboradoPor,
                               model.Opcion,
                               revisores,
                               aprobadores,
                               secundarios);
                            await DeleteFTPFileAsync(model.DocEditableRuta);
                        }
                        else
                        {
                            await _editarDocumento.Execute(
                                   model.IdSGADocumento,
                                   tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                                   model.IdSGATipoDocumento,
                                   model.DocCodigo,
                                   tipoDoc.TipoDocumento.Codigo,
                                   model.DocNombre,
                                   model.DocVersion,
                                   model.DocPredecesor,
                                   model.Comentario,
                                   cargaDocumentoPdf,
                                   model.DocEditableRuta,
                                   model.FechaTentativa,
                                   model.FechaAprobacion,
                                   model.IdElaboradoPor,
                                   model.Opcion,
                                   revisores,
                                   aprobadores,
                                   secundarios);
                        }
                        await DeleteFTPFileAsync(model.DocPdfRuta);
                    }
                    else
                    {
                        var nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
                        var cargaDocumentoEditable = await CargarDocumento(model.DocumentoEditable, nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);
                        await _editarDocumento.Execute(
                               model.IdSGADocumento,
                               tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                               model.IdSGATipoDocumento,
                               model.DocCodigo,
                               tipoDoc.TipoDocumento.Codigo,
                               model.DocNombre,
                               model.DocVersion,
                               model.DocPredecesor,
                               model.Comentario,
                               model.DocPdfRuta,
                               cargaDocumentoEditable,
                               model.FechaTentativa,
                               model.FechaAprobacion,
                               model.IdElaboradoPor,
                               model.Opcion,
                               revisores,
                               aprobadores,
                               secundarios);
                        await DeleteFTPFileAsync(model.DocEditableRuta);
                    }
                    if (model.Opcion.Equals("Registrar"))
                        //return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                        return RedirectToAction("NotificarNuevoDocumento", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()) });
                    else
                        return RedirectToAction("EditarDocumento", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                }
                else
                {
                    await _editarDocumento.Execute(
                            model.IdSGADocumento,
                            tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                            model.IdSGATipoDocumento,
                            model.DocCodigo,
                            tipoDoc.TipoDocumento.Codigo,
                            model.DocNombre,
                            model.DocVersion,
                            model.DocPredecesor,
                            model.Comentario,
                            model.DocPdfRuta,
                            model.DocEditableRuta,
                            model.FechaTentativa,
                            model.FechaAprobacion,
                            model.IdElaboradoPor,
                            model.Opcion,
                            revisores,
                            aprobadores,
                            secundarios);
                    if (model.Opcion.Equals("Registrar"))
                        return RedirectToAction("NotificarNuevoDocumento", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()) });
                    //return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                    else
                        return RedirectToAction("EditarDocumento", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()), IdCategoria = tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento });
                }
            }
            return View(model);
        }
        public async Task<IActionResult> EditarDocumentoSRevision(string IdDocumento, int IdCategoria, string Error)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Error=" + Error + "&IdCategoria=" + IdCategoria + "&IdDocumento=" + IdDocumento });
            }
            else
            {
                var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var tituloCategoria = "";
                switch (IdCategoria)
                {
                    case 1: tituloCategoria = "SGA"; break;
                    case 2: tituloCategoria = "SIG"; break;
                    case 3: tituloCategoria = "Externo"; break;
                }
                ViewBag.CategoriaDoc = tituloCategoria;
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
                    PdfRuta = x.PdfRuta,
                    EditableRuta = x.EditableRuta
                }).ToList();

                ViewBag.Secundarios = secundarios;
                /*
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
                */
                var model = new SGADocumentoModel
                {
                    IdEncriptado = IdDocumento,
                    IdSGADocumento = documento.IdSGADocumento,
                    IdElaboradoPor = documento.IdUsuario,
                    DocCodigo = documento.DocCodigo,
                    IdCategoria = IdCategoria,
                    IdSGATipoDocumento = documento.IdSGACategoriaTipo,
                    DocNombre = documento.DocNombre,
                    DocPredecesor = documento.IdDocPredecesor,
                    DocVersion = documento.DocVersion,
                    Comentario = documento.Comentario,
                    Condicion = documento.IdCondicion,
                    DocPdfRuta = documento.PdfRuta,
                    DocEditableRuta = documento.EditableRuta,
                    FechaTentativa = documento.FechaTentativa != null ? documento.FechaTentativa.Value.ToString("yyyy-MM-dd") : "",
                    FechaAprobacion = documento.FechaAprobacion != null ? documento.FechaAprobacion.Value.ToString("yyyy-MM-dd") : "",
                    //Revisores = JsonConvert.SerializeObject(revisores),
                    //Aprobadores = JsonConvert.SerializeObject(aprobadores),
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
                var revisores = model.Revisores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores) : new List<RevisionAprobacionInputOutput>();
                var aprobadores = model.Aprobadores != null ? JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores) : new List<RevisionAprobacionInputOutput>();
                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.IdSGATipoDocumento));
                var secundarios = JsonConvert.DeserializeObject<List<int>>("[]");
                if (model.Secundarios != null)
                    secundarios = JsonConvert.DeserializeObject<List<int>>(model.Secundarios);

                if (model.DocumentoPdf != null || model.DocumentoEditable != null)
                {
                    if (model.DocumentoPdf != null)
                    {
                        var nombreDocPdf = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoPdf.FileName);
                        var cargaDocumentoPdf = await CargarDocumento(model.DocumentoPdf, nombreDocPdf, tipoDoc.TipoDocumento.Descripcion);
                        if (model.DocumentoEditable != null)
                        {
                            var nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
                            var cargaDocumentoEditable = await CargarDocumento(model.DocumentoEditable, nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);
                            await _editarDocumento.Execute(
                               model.IdSGADocumento,
                               tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                               model.IdSGATipoDocumento,
                               model.DocCodigo,
                               tipoDoc.TipoDocumento.Codigo,
                               model.DocNombre,
                               model.DocVersion,
                               model.DocPredecesor,
                               model.Comentario,
                               cargaDocumentoPdf,
                               cargaDocumentoEditable,
                               model.FechaTentativa,
                               model.FechaAprobacion,
                               model.IdElaboradoPor,
                               model.Opcion,
                               revisores,
                               aprobadores,
                               secundarios);
                            await DeleteFTPFileAsync(model.DocEditableRuta);
                        }
                        else
                        {
                            await _editarDocumento.Execute(
                                   model.IdSGADocumento,
                                   tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                                   model.IdSGATipoDocumento,
                                   model.DocCodigo,
                                   tipoDoc.TipoDocumento.Codigo,
                                   model.DocNombre,
                                   model.DocVersion,
                                   model.DocPredecesor,
                                   model.Comentario,
                                   cargaDocumentoPdf,
                                   model.DocEditableRuta,
                                   model.FechaTentativa,
                                   model.FechaAprobacion,
                                   model.IdElaboradoPor,
                                   model.Opcion,
                                   revisores,
                                   aprobadores,
                                   secundarios);
                        }
                        await DeleteFTPFileAsync(model.DocPdfRuta);
                    }
                    else
                    {
                        var nombreDocEditable = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
                        var cargaDocumentoEditable = await CargarDocumento(model.DocumentoEditable, nombreDocEditable, tipoDoc.TipoDocumento.Descripcion);
                        await _editarDocumento.Execute(
                               model.IdSGADocumento,
                               tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                               model.IdSGATipoDocumento,
                               model.DocCodigo,
                               tipoDoc.TipoDocumento.Codigo,
                               model.DocNombre,
                               model.DocVersion,
                               model.DocPredecesor,
                               model.Comentario,
                               model.DocPdfRuta,
                               cargaDocumentoEditable,
                               model.FechaTentativa,
                               model.FechaAprobacion,
                               model.IdElaboradoPor,
                               model.Opcion,
                               revisores,
                               aprobadores,
                               secundarios);
                        await DeleteFTPFileAsync(model.DocEditableRuta);
                    }
                    if (model.Opcion.Equals("Registrar"))
                        //return RedirectToAction("NotificarNuevoDocumento", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()) });
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()) });
                    else
                        return RedirectToAction("EditarDocumentoSRevision", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()), IdCategoria = model.IdCategoria });
                }
                else
                {
                    await _editarDocumento.Execute(
                            model.IdSGADocumento,
                            tipoDoc.CategoriaDocumento.Id_SGA_Categoria_Documento,
                            model.IdSGATipoDocumento,
                            model.DocCodigo,
                            tipoDoc.TipoDocumento.Codigo,
                            model.DocNombre,
                            model.DocVersion,
                            model.DocPredecesor,
                            model.Comentario,
                            model.DocPdfRuta,
                            model.DocEditableRuta,
                            model.FechaTentativa,
                            model.FechaAprobacion,
                            model.IdElaboradoPor,
                            model.Opcion,
                            revisores,
                            aprobadores,
                            secundarios);
                    if (model.Opcion.Equals("Registrar"))
                        //return RedirectToAction("NotificarNuevoDocumento", "NotificacionesSGAAdministracion", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()) });
                        return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()) });
                    else
                        return RedirectToAction("EditarDocumentoSRevision", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()), IdCategoria = model.IdCategoria });
                }
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
                    filasTabla += "<td><a href=\"/SGAAdministracion/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a>";

                    filasTabla += " &nbsp; &nbsp;<a style=\"cursor:pointer\" onclick=\"this.disabled=true;PreviewPDF(\'" + fila.PdfRuta.Replace("\\", "\\\\") + "\');this.disabled=false;\">" +
                        "<svg xmlns=\"http://www.w3.org/2000/svg\"  viewBox=\"0 0 48 48\" width=\"24px\" height=\"24px\"><path fill=\"#616161\" d=\"M34.6 28.1H38.6V45.1H34.6z\" transform=\"rotate(-45.001 36.586 36.587)\"/><path fill=\"#616161\" d=\"M20 4A16 16 0 1 0 20 36A16 16 0 1 0 20 4Z\"/><path fill=\"#37474F\" d=\"M36.2 32.1H40.2V44.400000000000006H36.2z\" transform=\"rotate(-45.001 38.24 38.24)\"/><path fill=\"#64B5F6\" d=\"M20 7A13 13 0 1 0 20 33A13 13 0 1 0 20 7Z\"/><path fill=\"#BBDEFB\" d=\"M26.9,14.2c-1.7-2-4.2-3.2-6.9-3.2s-5.2,1.2-6.9,3.2c-0.4,0.4-0.3,1.1,0.1,1.4c0.4,0.4,1.1,0.3,1.4-0.1C16,13.9,17.9,13,20,13s4,0.9,5.4,2.5c0.2,0.2,0.5,0.4,0.8,0.4c0.2,0,0.5-0.1,0.6-0.2C27.2,15.3,27.2,14.6,26.9,14.2z\"/></svg>" +
                        "</a></td>";
                    filasTabla += "<td>" + fila.DocNombre + "</td>";
                    filasTabla += "<td>" + fila.DocVersion + "</td>";
                    filasTabla += "<td>" + (fila.FechaAprobacion.HasValue ? fila.FechaAprobacion.Value.ToString("dd/MM/yyyy") : "") + "</td>";
                    if (fila.IdSGACategoriaDocumento != 3)
                    {
                        filasTabla += "<td>" + fila.Condicion + "</td>";
                    }
                    filasTabla += "<td>" + fila.Comentario + "</td>";
                    filasTabla += "<td>" + (fila.FechaTentativa.HasValue ? fila.FechaTentativa.Value.ToString("dd/MM/yyyy") : "") + "</td>";
                    filasTabla += "</tr>";
                }
            }
            else
            {
                filasTabla = "<tr><td colspan=\"100%\">Sin datos</td></tr>";
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
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".ppt", "application/vnd.ms-powerpoint"},
                {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".csv", "text/csv"}
            };
        }
        private async Task<string> CargarDocumento(IFormFile file, string newFileName, string tipo)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre-v2/";
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
                    Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                    Log.Error(e.Message);
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
            try
            {
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);

                    sftp.DeleteFile(filename);

                    sftp.Dispose();
                }
            }
            catch (Exception e)
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                Log.Error(e.Message);
            }
        }
        public async Task<IActionResult> DescargarPlantilla(int IdCategoriaTipo)
        {

            var items = await _selectItems.Execute();
            var categoriaTipo = items.ItemsCategoriaTipo.First(x => x.IdSGACategoriaTipo.Equals(IdCategoriaTipo));
            var nombreDoc = Path.GetFileName(categoriaTipo.TipoDocumento.Formato);
            FileStream stream = new FileStream(Path.Combine(_hostingEnvironment.WebRootPath, categoriaTipo.TipoDocumento.Formato), FileMode.Open);
            return File(stream, GetContentType(nombreDoc), nombreDoc);
        }
        public async Task<IActionResult> AmpliarFecha(string IdRevision, string NuevaFecha)
        {
            dynamic response;
            try
            {
                var IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdRevision));
                await _ampliarFechaRevision.Execute(IdDesencriptado, NuevaFecha);
                response = new
                {
                    success = true
                };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    error = e.Message
                };

            }
            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult> DeleteDocument(int idDocuemnet)
        {
            await _deleteDocument.Execute(idDocuemnet);

            return Ok();
        }
        //public async Task<IActionResult> FirmarDocumento(string IdDocumento, int IdCategoria, string Error)
        //{
        //    if (!(HttpContext.Session.GetString("SessionUser") != null))
        //    {
        //        return RedirectToAction("NoAutorizado", "Home");
        //    }
        //    else
        //    {
        //        var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
        //        var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

        //        ViewBag.Rol = userInfo.Rol.ToString();
        //        ViewBag.Usuario = userInfo.NombreUsuario;

        //        var usuarioActual = await _getUsuario.Execute(userInfo.NombreUsuario);

        //        var items = await _selectItems.Execute();
        //        var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
        //        condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

        //        var tipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(IdCategoria)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
        //        tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

        //        var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
        //        tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

        //        var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

        //        ViewBag.Usuarios = usuarios;
        //        ViewBag.Condiciones = condiciones;
        //        ViewBag.TiposDoc = tipos;
        //        ViewBag.TiposRevision = tiposRevision;
        //        ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

        //        var documento = await _consultarDocumento.Execute(IdDesencriptado);
        //        //Obtener documentos que tienen como documento principal el actual
        //        var documentosSecundarios = await _consultarDocumentos.ExecuteSecundarios(IdDesencriptado);
        //        var secundarios = documentosSecundarios.Select(x => new
        //        {
        //            IdEncriptado = _protector.Protect(x.IdSGADocumento.ToString()),
        //            Codigo = x.DocCodigo,
        //            TipoDocumento = x.SGATipoDocumento,
        //            Nombre = x.DocNombre,
        //            Version = x.DocVersion,
        //            Condicion = x.Condicion,
        //            PdfRuta = x.PdfRuta,
        //            EditableRuta = x.EditableRuta
        //        }).ToList();
        //        /*
        //        var revisores = documento.Revisores.Select(x => new {
        //            Orden = x.Orden,
        //            Usuario = x.Usuario.IdUsuario,
        //            UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
        //            Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd")
        //        })
        //            .ToList();

        //        var aprobadores = documento.Aprobadores.Select(x => new {
        //            Orden = x.Orden,
        //            Usuario = x.Usuario.IdUsuario,
        //            UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
        //            Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"),
        //        })
        //            .ToList();
        //        */
        //        var model = new SGADocumentoModel
        //        {
        //            IdEncriptado = IdDocumento,
        //            IdSGADocumento = documento.IdSGADocumento,
        //            IdCategoria = IdCategoria,
        //            IdSGATipoDocumento = documento.IdSGACategoriaTipo,
        //            DocNombre = documento.DocNombre,
        //            DocPredecesor = documento.IdDocPredecesor,
        //            DocCodigo = documento.DocCodigo,
        //            DocVersion = documento.DocVersion,
        //            Comentario = documento.Comentario,
        //            Condicion = documento.IdCondicion,
        //            DocPdfRuta = documento.PdfRuta,
        //            DocEditableRuta = documento.EditableRuta,
        //            FechaTentativa = documento.FechaTentativa != null ? documento.FechaTentativa.Value.ToString("yyyy-MM-dd") : "",
        //            //Revisores = JsonConvert.SerializeObject(revisores),
        //            //Aprobadores = JsonConvert.SerializeObject(aprobadores),
        //            Secundarios = JsonConvert.SerializeObject(secundarios)
        //        };

        //        ViewBag.Error = Error;

        //        return View(model);
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> FirmarDocumento(SGADocumentoModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

        //        var items = await _selectItems.Execute();
        //        var tipoDoc = items.ItemsCategoriaTipo.Find(x => x.IdSGACategoriaTipo.Equals(model.IdSGATipoDocumento));
        //        var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.DocNombre + "_v." + String.Format("{0:0.00}", model.DocVersion) + Path.GetExtension(model.DocumentoEditable.FileName);
        //        var cargaDocumento = await CargarDocumento(model.DocumentoEditable, nombreDoc, tipoDoc.TipoDocumento.Descripcion);
        //        if (!cargaDocumento.Equals("Error"))
        //        {
        //                await _firmarDocumento.Execute(
        //                    model.IdSGADocumento,
        //                    cargaDocumento,
        //                    tipoDoc.TipoDocumento.Codigo,
        //                    model.DocCodigo,
        //                    tipoDoc.IdSGACategoriaTipo);
        //            return RedirectToAction("VerDocumento", new { IdDocumento = _protector.Protect(model.IdSGADocumento.ToString()) });
        //        }
        //        return RedirectToAction("FirmarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
        //    }
        //    return View(model);
        //}
    }
}
