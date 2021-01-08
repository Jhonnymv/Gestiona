using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.SGA_Documentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarPredecesores;
using GestionAmbiental.Application.UseCases.SGA_Documentos.Items;
using GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarRevisorAprobador;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
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
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarRevisionDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.EditarRevisionDocumento;

namespace GestionAmbiental.UseCases.SGA_Documentos
{
    public class SGADocumentosController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IConsultarDocumentos _consultarDocumentos;
        private readonly IConsultarDocumento _consultarDocumento;
        private readonly IConsultarRevisionDocumento _consultarRevisionDocumento;
        private readonly IGetUsuario _getUsuario;
        private readonly IRegistrarDocumento _registrarDocumento;
        private readonly IConsultarPredecesores _consultarPredecesores;
        private readonly IEditarRevisionDocumento _editarRevisionDocumento;
        private readonly IRegistrarRevisorAprovador _registrarRevisorAprovador;
        private readonly IGetInfo_SGA _info_SGA;

        private readonly IDataProtector _protector;

        public SGADocumentosController(
            ISelectItems selectItems, 
            IConsultarDocumentos consultarDocumentos,
            IConsultarDocumento consultarDocumento,
            IConsultarRevisionDocumento consultarRevisionDocumento,
            IGetUsuario getUsuario,
            IRegistrarDocumento registrarDocumento,
            IConsultarPredecesores consultarPredecesores,
            IEditarRevisionDocumento editarRevisionDocumento,
            IRegistrarRevisorAprovador registrarRevisorAprovador,
            IGetInfo_SGA info_SGA,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _selectItems = selectItems;
            _consultarDocumentos = consultarDocumentos;
            _consultarDocumento = consultarDocumento;
            _consultarRevisionDocumento = consultarRevisionDocumento;
            _getUsuario = getUsuario;
            _consultarPredecesores = consultarPredecesores;
            _editarRevisionDocumento = editarRevisionDocumento;
            _registrarRevisorAprovador = registrarRevisorAprovador;
            _registrarDocumento = registrarDocumento;
            _info_SGA = info_SGA;
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
                var condiciones = items.ItemsCondicion.Where(x=>x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGACondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipos = items.ItemsTipoDocumento.Where(x=>x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdSGATipoDocumento.ToString(), Text = g.Descripcion }).ToList();
                tipos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string nombre, int tipo, string codigo, int condicion, int pagina, int rpp)
        {
            var datosTabla = await _consultarDocumentos.Execute(nombre,tipo,codigo,condicion,pagina,rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Documentos)
                {
                    var id_encriptado = _protector.Protect(fila.IdSGADocumento.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/SGADocumentos/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
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
                    Orden = x.Orden, Usuario = x.Usuario.IdUsuario, 
                    UsuarioNombre = x.Usuario.Nombres + " " + x.Usuario.Apellidos, 
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"), 
                    Estado = x.Estado,
                    Permiso = x.Usuario.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false })
                    .ToList();

                var aprobadores = documento.Aprobadores.Select(x => new { 
                    Id = _protector.Protect(x.IdSGARevisionDocumento.ToString()), 
                    Orden = x.Orden, Usuario = x.Usuario.IdUsuario, 
                    UsuarioNombre = x.Usuario.Nombres +" "+x.Usuario.Apellidos,
                    Fecha = x.FechaLimite.Value.ToString("yyyy-MM-dd"), 
                    Estado = x.Estado, 
                    Permiso = x.Usuario.IdUsuario.Equals(usuarioActual.IdUsuario) ? true : false })
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
                    filasTabla += "<td><a href=\"/SGADocumentos/VerDocumento?IdDocumento=" + id_encriptado + "\">" + fila.DocCodigo + "</a></td>";
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
        public async Task<IActionResult> RegistrarDocumento(string Error)
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
                
                var tiposRevision = items.ItemsTipoRevision.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdTipoRevision.ToString(), Text = g.Descripcion }).ToList();
                tiposRevision.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.Where(x => x.Estado.Equals("ACT")).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
               
                ViewBag.Condiciones = condiciones;
                ViewBag.TiposDoc = tipos;
                ViewBag.Usuarios = usuarios;
                ViewBag.TiposRevision = tiposRevision;
                ViewBag.FechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

                //Obtener ultimas versiones de documentos publicados
                var documentos = await _consultarDocumentos.Execute();
                var documentosAgrupados = documentos.GroupBy(x => x.SGATipoDocumento).ToList();

                var predecesoresTipo = new List<SelectListItem>();
                foreach (var grupo in documentosAgrupados)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Key, Disabled = false };
                    foreach (var docPredecesor in grupo)
                        predecesoresTipo.Add(new SelectListItem() { Value = docPredecesor.IdSGADocumento.ToString(), Text = "(" + docPredecesor.DocCodigo + ") " + docPredecesor.DocNombre+" v."+docPredecesor.DocVersion.ToString(), Group = nuevoGrupo });
                }
                predecesoresTipo.Insert(0, new SelectListItem("Ninguno", "0", true));

                ViewBag.Predecesores = predecesoresTipo;
                ViewBag.Error = Error;


                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegistrarDocumento(SGADocumentoModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var items = await _selectItems.Execute();
                var tipoDoc = items.ItemsTipoDocumento.Find(x => x.IdSGATipoDocumento.Equals(model.Id_SGA_Tipo_Documento));
                var nombreDoc = DateTime.Now.ToString("ddMMyyyyHHmmss") + "_" + model.Doc_Nombre + "_v." + String.Format("{0:0.0}", model.Doc_Version) + Path.GetExtension(model.Documento.FileName);
                var revisores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Revisores);
                var aprobadores = JsonConvert.DeserializeObject<List<RevisionAprobacionInputOutput>>(model.Aprobadores);
                var cargaDocumento = await CargarDocumento(model.Documento, nombreDoc, tipoDoc.Descripcion);

                if (!cargaDocumento.Equals("Error"))
                {                  
                    var idDocumento = await _registrarDocumento.Execute(
                        model.Id_SGA_Tipo_Documento,
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
                    return RedirectToAction("RegistrarDocumento", new { Error = "Ocurrió un error al cargar el archivo" });
                }

            }
            return View(model);
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
        private async Task<string> CargarDocumento(IFormFile file, string newFileName,string tipo)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

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
        public async Task<string> UploadFTPFileAsync(string path, string newFileName,string tipo)
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
                catch(Exception e)
                {
                    return "Error";
                }
            }
            else
                return "Error";
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
                    var encryptedFile = await DownloadFTPFileAsync(fileName,tipoDoc,nombreDoc);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\"+tipoDoc+"\\";

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
        public async Task<string> DownloadFTPFileAsync(string fileName,string tipoDoc, string nombreDoc)
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
                    sftp.ChangeDirectory(dir+tipoDoc);

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
        private async Task DeleteFTPFileAsync(string filename)
        {
            var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionAvistamientoFauna);
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
                sftp.ChangeDirectory(dir);

                sftp.DeleteFile(filename);

                sftp.Dispose();
            }
        }
    }
}
