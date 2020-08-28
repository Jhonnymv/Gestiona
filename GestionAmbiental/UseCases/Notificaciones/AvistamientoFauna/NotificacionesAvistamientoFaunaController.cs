using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientoFauna;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.AvistamientoFauna.GetUsuariosNotificar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.AvistamientoFauna.DescargarAvistamientoFauna;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Dml.Chart;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.Notificaciones.AvistamientoFauna
{
    public class NotificacionesAvistamientoFaunaController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetAvistamientoFauna _getAvistamientoFauna;
        private readonly IGetUsuariosNotificar _getUsuariosNotificar;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;
        private readonly DescargarAvistamientoFaunaController _descargarAvistamientoFaunaController;

        public NotificacionesAvistamientoFaunaController(
           IHostingEnvironment hostingEnvironment,
           IGetAvistamientoFauna getAvistamientoFauna,
           IGetUsuariosNotificar getUsuariosNotificar,
           IGetInfo_SGA getInfo_SGA,
           IDataProtectionProvider dataProtectionProvider,
           DataProtectionPurpouseStrings dataProtectionPurpouseStrings, 
           DescargarAvistamientoFaunaController descargarAvistamientoFaunaController
           )
        {
            _hostingEnvironment = hostingEnvironment;
            _getAvistamientoFauna = getAvistamientoFauna;
            _getUsuariosNotificar = getUsuariosNotificar;
            _info_SGA = getInfo_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.AvistamientoFaunaIdRouteValue);
            _descargarAvistamientoFaunaController = descargarAvistamientoFaunaController;
        }

        public async Task<IActionResult> NotificarNuevoAvistamiento(string IdAvistamiento)
        {
            var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdAvistamiento));
            var avistamiento = await _getAvistamientoFauna.Execute(IdDesencriptado);

            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "AvistamientoFauna", "NuevoAvistamientoFauna.html");

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
                IdAvistamiento,
                avistamiento.Codigo,
                avistamiento.FechaAvistamiento.ToString("dd/MM/yyyy"),
                avistamiento.FechaAvistamiento.ToString("HH:mm"),
                avistamiento.Ubicacion,
                avistamiento.NombreComun,
                avistamiento.Cantidad,
                avistamiento.TipoAvistamiento,
                avistamiento.EstadoFauna,
                avistamiento.LugarReferencia,
                avistamiento.Empresa,
                avistamiento.Usuario,
                avistamiento.AreaTrabajo,
                avistamiento.DNI
                );
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            var usuariosNotificar = await _getUsuariosNotificar.Execute();
            foreach(var usuario in usuariosNotificar)
            {
                msg.To.Add(new MailAddress(usuario.Email));
            }
            
            msg.Subject = "Nuevo Avistamiento de Fauna";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            var archivoAvistamiento = await _descargarAvistamientoFaunaController.PrepararAvistamiento(avistamiento.IdAvistamientoFauna);
            msg.Attachments.Add(new Attachment(archivoAvistamiento, "AvistamientoFauna-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx"));

            await mail.SendMailAsync(msg);

            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("DetalleRegistro", "AvistamientoFauna", new { IdAvistamiento = IdAvistamiento });
        }
        public async Task<IActionResult> NotificarAccionAvistamiento(string IdAvistamiento)
        {
            var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdAvistamiento));
            var avistamiento = await _getAvistamientoFauna.Execute(IdDesencriptado);

            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "AvistamientoFauna", "NuevaAccionAvistamientoFauna.html");

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
                IdAvistamiento,
                avistamiento.Codigo,
                avistamiento.FechaAvistamiento.ToString("dd/MM/yyyy"),
                avistamiento.FechaAvistamiento.ToString("HH:mm"),
                avistamiento.Ubicacion,
                avistamiento.NombreComun,
                avistamiento.Cantidad,
                avistamiento.TipoAvistamiento,
                avistamiento.EstadoFauna,
                avistamiento.LugarReferencia,
                avistamiento.Empresa,
                avistamiento.Usuario,
                avistamiento.AreaTrabajo,
                avistamiento.DNI,
                avistamiento.DescripcionAccion
                );
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            msg.To.Add(new MailAddress(avistamiento.UsuarioEmail));

            msg.Subject = "Nueva Acción de Avistamiento de Fauna";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            var archivoAvistamiento = await _descargarAvistamientoFaunaController.PrepararAvistamiento(avistamiento.IdAvistamientoFauna);
            msg.Attachments.Add(new Attachment(archivoAvistamiento, "AvistamientoFauna-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx"));

            for (int i = 0; i < avistamiento.EvidenciaAccion.Length; i++)
            {
                var rutaArchivo = await Download(avistamiento.EvidenciaAccion[i], i + 1, "EvidenciaAccion");
                if (rutaArchivo != null)
                    msg.Attachments.Add(new Attachment(rutaArchivo));
            }

            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("DetalleRegistro", "AvistamientoFauna", new { IdAvistamiento = IdAvistamiento });
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
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }
        public async Task<string> Download(string fileName, int nImagen,string tipo)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var encryptedFile = await DownloadFTPFileAsync(fileName,tipo);

                /* Save encrypted file in temp path*/
                var tempPath = Path.GetTempPath();

                var SGATempPath = tempPath + "SGA-Marcobre\\";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                /*decrypt file*/
                var decryptedFileName = tipo + nImagen + Path.GetExtension(fileName);
                await DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);

                return archivePath + decryptedFileName;
            }
            else
                return null;
        }
        public async Task<string> DownloadFTPFileAsync(string fileName,string tipo)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var rutaSFTP = tipo == "EvidenciaAccion" ? Tipo_Info_SGA.FTPConnectionAvistamientoFaunaAccion : Tipo_Info_SGA.FTPConnectionAvistamientoFauna;

            var serverInfo = await _info_SGA.Execute(rutaSFTP);
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

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            if (!Directory.Exists(archivePath))
            {
                Directory.CreateDirectory(archivePath);
            }

            /*New file downloaded name*/
            var downloadedFilePath = archivePath + fileName;
            /*Copy data to new temp file*/
            var fileStream = System.IO.File.Create(downloadedFilePath);
            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);

                //Descargar archivo de sftp a nuevo archivo temporal creado
                sftp.DownloadFile(fileName, fileStream);

                sftp.Dispose();
            }
            fileStream.Close();

            return downloadedFilePath;
        }
        private async Task EncryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, IV),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);

            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        private async Task DecryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }
            RijndaelManaged RMCrypto = new RijndaelManaged();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, IV),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

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
