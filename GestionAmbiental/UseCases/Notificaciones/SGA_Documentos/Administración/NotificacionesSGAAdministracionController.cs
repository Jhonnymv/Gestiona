using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.SGA_Documentos;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumento;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.Notificaciones.SGADocumentos.GetUsuariosNotificar;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Renci.SshNet;
using Serilog;

namespace GestionAmbiental.UseCases.Notificaciones.SGA_Documentos.Administración
{
    public class NotificacionesSGAAdministracionController : Controller
    {
        private readonly IGetUsuariosNotificar _getUsuariosNotificar;
        private readonly IConsultarDocumento _consultarDocumento;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IGetUsuario _getUsuario;
        private readonly IDataProtector _protector;
        private IHostingEnvironment _hostingEnvironment;
        public NotificacionesSGAAdministracionController(IConsultarDocumento consultarDocumento,
            IGetUsuariosNotificar getUsuariosNotificar,
             IGetInfo_SGA info_SGA,
             IGetUsuario getUsuario,
            IHostingEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _consultarDocumento = consultarDocumento;
            _getUsuariosNotificar = getUsuariosNotificar;
            _info_SGA = info_SGA;
            _getUsuario = getUsuario;
            _hostingEnvironment = hostingEnvironment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.DocumentosSGAIdRouteValue);
        }
        /*
        public async Task<IActionResult> NotificarRevisionActiva(string IdDocumento,int IdCategoria)
        {
            //Buscar documento
            var idDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
            var documento = await _consultarDocumento.Execute(idDesencriptado);
            //Buscar revision activa
            //Obtener correo del usuario a notificar
            string correo = "";
            string fechaVencimiento = "";
            string usuarioResponsable = "";
            var revisionActiva = documento.Revisores.FirstOrDefault(x => x.Estado.Equals(EstadosRevisionDocSGA.Activo.ToString()));
            if (revisionActiva == null)
            {
                var aprobacionActiva = documento.Aprobadores.FirstOrDefault(x => x.Estado.Equals(EstadosRevisionDocSGA.Activo.ToString()));
                correo = aprobacionActiva.Usuario.Email;
                fechaVencimiento = aprobacionActiva.FechaLimite.Value.ToString("dd/MM/yyyy");
                usuarioResponsable = aprobacionActiva.Usuario.Nombres + " " + aprobacionActiva.Usuario.Apellidos;
            }
            else
            {
                correo = revisionActiva.Usuario.Email;
                fechaVencimiento = revisionActiva.FechaLimite.Value.ToString("dd/MM/yyyy");
                usuarioResponsable = revisionActiva.Usuario.Nombres + " " + revisionActiva.Usuario.Apellidos;
            }
            //Editar plantilla del correo
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "SGA_Documentos", "NuevaRevision.html");

            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                IdDocumento,
                documento.DocCodigo,
                documento.DocNombre,
                documento.DocVersion.ToString(),
                documento.Comentario,
                documento.FechaCreacion.ToString("dd/MM/yyyy"),
                usuarioResponsable,
                fechaVencimiento
                );

            var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
            string smtp = "", port = "", user = "", pass = "", mailFrom = "";
            foreach (var item in credenciales.Info)
            {
                switch (item.Key)
                {
                    case "smtp": smtp = item.Value; break;
                    case "ntwkuser": user = item.Value; break;
                    case "ntwkpassword": pass = item.Value; break;
                    case "port": port = item.Value; break;
                    case "mailfrom": mailFrom = item.Value; break;
                }
            }

            //Enviar correo
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.Subject = "Nueva Revisión de Documento";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            msg.To.Add(new MailAddress(correo));

            await mail.SendMailAsync(msg);
            mail.Dispose();
            msg.Dispose();
            //Solo notifica SGI
            return RedirectToAction("VerDocumento","SGAAdministracion", new { IdDocumento = _protector.Protect(documento.IdSGADocumento.ToString()), IdCategoria = 1 });
        }
        */
        public async Task<IActionResult> NotificarNuevoDocumento(string IdDocumento)
        {
            var r = 45;
            var g = 100;
            var b = 150;
            var img_url = "/img/back-inspection.png";
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("SGA"));
                if (dataModulo != null)
                {
                    r = dataModulo.r;
                    g = dataModulo.g;
                    b = dataModulo.b;
                    img_url = dataModulo.img_url;
                }
            }
            catch (Exception e)
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                Log.Error(e.Message);
            }
            var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdDocumento));
            var documento = await _consultarDocumento.Execute(IdDesencriptado);

            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "SGA_Documentos", "NuevoDocumento.html");

            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
            string smtp = "", port = "", user = "", pass = "", mailFrom = "";
            foreach (var item in credenciales.Info)
            {
                switch (item.Key)
                {
                    case "smtp": smtp = item.Value; break;
                    case "ntwkuser": user = item.Value; break;
                    case "ntwkpassword": pass = item.Value; break;
                    case "port": port = item.Value; break;
                    case "mailfrom": mailFrom = item.Value; break;
                }
            }
            var msgBody = string.Format(templateReaded,
                $"#{r:X2}{g:X2}{b:X2}",
                documento.DocCodigo,
                documento.DocNombre,
                documento.DocVersion,
                documento.SGACategoriaDocumento,
                documento.SGATipoDocumento,
                documento.Usuario,
                documento.FechaAprobacion.HasValue ? documento.FechaAprobacion.Value.ToString("dd/MM/yyyy") : "No Aplica",
                IdDocumento
                );
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            var usuariosNotificar = await _getUsuariosNotificar.Execute();

            var archivo = await GetFileStream(documento.PdfRuta);
            if (archivo != "Error" || archivo != "No inició sesión")
            {
                FileStream streamPdf = new FileStream(archivo, FileMode.Open);
                msg.Attachments.Add(new Attachment(streamPdf, documento.DocNombre +" v." + documento.DocVersion + Path.GetExtension(documento.PdfRuta)));
            }
            msg.Subject = "Nuevo Documento " + documento.SGACategoriaDocumento;
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            var correosUsuarios = new List<MailAddress>();
            for (int i = 0; i < usuariosNotificar.Count; i++)
            {
                msg.To.Add(new MailAddress(usuariosNotificar[i].Email));
                correosUsuarios.Add(new MailAddress(usuariosNotificar[i].Email));
                if (i > 0 && i % 20 == 0)
                {
                    await mail.SendMailAsync(msg);

                    foreach (var item in correosUsuarios)
                        msg.To.Remove(item);

                    correosUsuarios = new List<MailAddress>();

                    Thread.Sleep(60000);
                }
            }
            if (correosUsuarios.Count > 0)
                await mail.SendMailAsync(msg);

            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("VerDocumento", "SGADocumentacion", new { IdDocumento = IdDocumento, IdCategoria = documento.IdSGACategoriaDocumento });
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
        public async Task<string> GetFileStream(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "No inició sesión";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                try
                {
                    var tipoDoc = fileName.Split('\\', 2)[0];
                    var nombreDoc = fileName.Split('\\', 2)[1];
                    var encryptedFile = await DownloadFTPFileAsync(fileName, tipoDoc, nombreDoc);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre-v2\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\" + tipoDoc + "\\";
                    return archivePath + nombreDoc;
                    /*read decrypted file*/
                    //FileStream stream = new FileStream(archivePath + nombreDoc, FileMode.Open);

                    //return File(stream, GetContentType(nombreDoc), fileName);
                }
                catch (Exception e)
                {
                    return "Error";
                }
            }
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
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
    }
}
