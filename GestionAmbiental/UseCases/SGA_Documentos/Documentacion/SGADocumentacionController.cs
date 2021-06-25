using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.SGA_Documentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarPredecesores;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarRevisionDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.EditarRevisionDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.Items;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.SGA_Documentos.Documentacion
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
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IHostingEnvironment _hostingEnvironment;

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
            IHostingEnvironment hostingEnvironment,
            IGetInfo_SGA info_SGA,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _selectItems = selectItems;
            _consultarDocumentos = consultarDocumentos;
            _consultarPredecesores = consultarPredecesores;
            _consultarDocumento = consultarDocumento;
            _consultarRevisionDocumento = consultarRevisionDocumento;
            _editarRevisionDocumento = editarRevisionDocumento;
            _getUsuario = getUsuario;
            _info_SGA = info_SGA;

            _hostingEnvironment = hostingEnvironment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.DocumentosSGAIdRouteValue);
        }
        public async Task<IActionResult> PropiedadesModulo()
        {
            dynamic response;
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("SGA"));
                if (dataModulo != null)
                    response = new
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(dataModulo)
                    };
                else
                    response = new
                    {
                        success = false,
                        msg = "No se encontraron las propiedades del módulo"
                    };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    msg = e.Message
                };
            }
            return Ok(response);
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

                var categorias = items.ItemsCategoriaDocumento.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.Id_SGA_Categoria_Documento.ToString(), Text = g.Descripcion }).ToList();

                var categoriasTipo = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT")).ToList();

                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.TiposDocJson = JsonConvert.SerializeObject(items.ItemsTipoDocumento);

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string nombre, int categoria, int[] tipos, string codigo, int condicion, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute(nombre, categoria, tipos, codigo, condicion, pagina, rpp);

            //datosTabla.NRegistros = datosTabla.Documentos.Count();
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
                    filasTabla += "<td>";
                    filasTabla += fila.FechaAprobacion.HasValue ? fila.FechaAprobacion.Value.ToString("dd/MM/yyyy") : "";
                    filasTabla += "</td>";
                    filasTabla += "<td>" + fila.Condicion + "</td>";
                    filasTabla += "<td>" + fila.Comentario + "</td>";
                    filasTabla += "<td>";
                    filasTabla += fila.FechaTentativa.HasValue ? fila.FechaTentativa.Value.ToString("dd/MM/yyyy") : "";
                    filasTabla += "</td>";
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
                filasTabla = "<tr><td colspan=\"100%\">Sin datos</td></tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetTablaModalResumenFiltrada(int categoria, int[] tipos, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute("", categoria, tipos, "", 0, pagina, rpp);
            datosTabla.Documentos = datosTabla.Documentos
                .Where(x => !x.Estado.Equals(EstadosDocumentosSGA.Pendiente.ToString()) &&
                !x.Estado.Equals(EstadosDocumentosSGA.Superado.ToString()))
                .OrderByDescending(x => x.FechaCreacion)
                .ToList();
            datosTabla.NRegistros = datosTabla.Documentos.Count();
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

                var revisores = documento.Revisores.Select(x => new
                {
                    //Id = _protector.Protect(x.IdSGARevisionDocumento.ToString()),
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaRevision.HasValue ? x.FechaRevision.Value.ToString("yyyy-MM-dd") : ""
                    //Estado = x.Estado,
                    ////El responsable es el usuario conectado?
                    //Permiso = x.Usuario.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false
                })
                    .ToList();

                var aprobadores = documento.Aprobadores.Select(x => new
                {
                    //Id = _protector.Protect(x.IdSGARevisionDocumento.ToString()),
                    Orden = x.Orden,
                    Usuario = x.Usuario.IdUsuario,
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos,
                    Fecha = x.FechaRevision.HasValue ? x.FechaRevision.Value.ToString("yyyy-MM-dd") : ""
                    //Estado = x.Estado,
                    ////El responsable es el usuario conectado?
                    //Permiso = x.Usuario.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false
                })
                    .ToList();

                var items = await _selectItems.Execute();
                var condiciones = items.ItemsCondicion.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var categoriasTipos = items.ItemsCategoriaTipo.Where(x => x.Estado.Equals("ACT") && x.CategoriaDocumento.Id_SGA_Categoria_Documento.Equals(documento.IdSGACategoriaDocumento)).Select(g => new SelectListItem() { Value = g.IdSGACategoriaTipo.ToString(), Text = g.TipoDocumento.Descripcion }).ToList();
                categoriasTipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                //Obtener documentos que tienen como documento principal el actual
                var documentosSecundarios = await _consultarDocumentos.ExecuteSecundarios(IdDesencriptado);

                var usuarios = items.ItemsUsuarios.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var secundarios = documentosSecundarios.Select(x => new
                {
                    IdEncriptado = _protector.Protect(x.IdSGADocumento.ToString()),
                    Codigo = x.DocCodigo,
                    TipoDocumento = x.SGATipoDocumento,
                    Nombre = x.DocNombre,
                    Version = x.DocVersion,
                    Condicion = x.Condicion,
                    PdfRuta = x.PdfRuta,
                    EditableRuta = x.EditableRuta,
                }).ToList();

                ViewBag.Usuarios = usuarios;
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = categoriasTipos;
                ViewBag.CategoriaDoc = documento.SGACategoriaDocumento;

                var model = new SGADocumentoModel
                {
                    IdEncriptado = IdDocumento,
                    IdSGADocumento = documento.IdSGADocumento,
                    IdElaboradoPor = documento.IdUsuario,
                    IdSGATipoDocumento = documento.IdSGACategoriaTipo,
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
                    filasTabla += "<td>" + (fila.FechaAprobacion.HasValue ? fila.FechaAprobacion.Value.ToString("dd/MM/yyyy") : "") + "</td>";
                    filasTabla += "<td>" + fila.Condicion + "</td>";
                    filasTabla += "<td>" + fila.Comentario + "</td>";
                    filasTabla += "<td>" + (fila.FechaTentativa.HasValue ? fila.FechaTentativa.Value.ToString("dd/MM/yyyy") : "") + "</td>";
                    filasTabla += "</tr>";
                }
            }
            else
            {
                filasTabla = "<tr><td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td></tr>";
            }
            dynamic response = new
            {
                Data = filasTabla
            };
            return Ok(response);
        }
        /*
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
                        success = false,
                        message = "El usuario actualmente conectado no es el mismo al usuario asignado a esta revisión"
                    };
                    return Ok(response);
                }
                else
                {
                    //Validar si se puede enviar la variable "documentoRevision" a la notificacion, incluir comentario
                    
                    if (!Opcion.Equals("Aceptar"))
                    {
                        //Editar
                        await _editarRevisionDocumento.Execute(IdDocumentoInt, IdDocumentoRevisionInt, Comentario, Opcion);
                        //Notificar
                        return RedirectToAction("NotificarRevisionRechazada", "NotificacionesSGADocumentacion", new { IdSGADocumento=documentoRevision.IdSGADocumento, UsuarioNombre= documentoRevision.UsuarioNombre, Comentario= Comentario });
                    }
                    else
                    {
                        //Editar
                        await _editarRevisionDocumento.Execute(IdDocumentoInt, IdDocumentoRevisionInt, Comentario, Opcion);
                        //Notificar
                        return RedirectToAction("NotificarRevisionAprobada", "NotificacionesSGADocumentacion", new { IdSGADocumento = documentoRevision.IdSGADocumento, UsuarioNombre = documentoRevision.UsuarioNombre, Comentario = Comentario });
                    }
                    
                    await _editarRevisionDocumento.Execute(IdDocumentoInt, IdDocumentoRevisionInt, Comentario, Opcion);

                    response = new
                    {
                        success = true,

                    };
                    return Ok(response);
                }
            }
        }
        */
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
        public async Task<IActionResult> Download(string fileName)
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
                try
                {
                    var tipoDoc = fileName.Split('/', 2)[0];
                    var nombreDoc = fileName.Split('/', 2)[1];
                    var encryptedFile = await DownloadFTPFileAsync(fileName, tipoDoc, nombreDoc);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\" + tipoDoc + "\\";

                    /*read decrypted file*/
                    FileStream stream = new FileStream(archivePath + nombreDoc, FileMode.Open);

                    return File(stream, GetContentType(nombreDoc), fileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }
        public async Task<IActionResult> PreviewFile(string fileName)
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
                dynamic response;
                try
                {
                    var tipoDoc = fileName.Split('\\', 2)[0];
                    var nombreDoc = fileName.Split('\\', 2)[1];

                    var downloadedFile = await DownloadFTPFileAsync(fileName, tipoDoc, nombreDoc);

                    var docsDownloadedPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Temp\DownloadedDocs");
                    if (Directory.Exists(docsDownloadedPath))
                    {
                        Directory.Delete(docsDownloadedPath, true);
                        Directory.CreateDirectory(Path.Combine(docsDownloadedPath, userInfo.NombreUsuario));
                    }
                    var fileStreamDownloaded = new FileStream(downloadedFile, FileMode.Open);
                    var docPath = Path.Combine(docsDownloadedPath, userInfo.NombreUsuario, nombreDoc);

                    using (var fileStream = new FileStream(docPath, FileMode.Create))
                    {
                        await fileStreamDownloaded.CopyToAsync(fileStream);
                        fileStream.Close();
                    }
                    fileStreamDownloaded.Close();
                    response = new
                    {
                        result = "Ok",
                        path = "/Temp/DownloadedDocs/" + userInfo.NombreUsuario + "/" + nombreDoc,
                        extension = Path.GetExtension(nombreDoc)
                    };
                    return Ok(response);
                }
                catch (Exception e)
                {
                    response = new
                    {
                        result = "Error",
                        message = "No se encontró el archivo: " + e.Message
                    };
                    return Ok(response);
                }
            }
        }
        public async Task<IActionResult> DownloadPreviewFile(string path)
        {
            var completePath = _hostingEnvironment.WebRootPath + "/" + path;
            FileStream stream = new FileStream(completePath, FileMode.Open);
            return File(stream, GetContentType(path), Path.GetFileName(path));
        }
        public async Task<string> DownloadFTPFileAsync(string fileName, string tipoDoc, string nombreDoc)
        {
            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

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
                /* Save file in application server to decrypt */
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
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
                Directory.CreateDirectory(archivePath + tipoDoc);

                /*New file downloaded name*/
                var downloadedFilePath = archivePath + fileName;
                /*Copy data to new temp file*/
                var fileStream = System.IO.File.Create(downloadedFilePath);
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir + tipoDoc);

                    //Descargar archivo de sftp a nuevo archivo temporal creado
                    sftp.DownloadFile(nombreDoc, fileStream);

                    sftp.Dispose();
                }
                fileStream.Close();

                return downloadedFilePath;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
