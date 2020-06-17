using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.ImplementarAccion;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.Incidentes.InvestigacionFinal.ImplementarAccion
{
    public class ImplementarAccionInvestigacionController : Controller
    {
        private readonly IGetAccioIncidenteInvestigacion _getAccioIncidenteInvestigacion;
        private readonly IImplementarAccion _implementarAccion;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;

        public ImplementarAccionInvestigacionController(
            IGetAccioIncidenteInvestigacion getAccioIncidenteInvestigacion,
            IImplementarAccion implementarAccion,
            IGetInfo_SGA info_SGA,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getAccioIncidenteInvestigacion = getAccioIncidenteInvestigacion;
            _implementarAccion = implementarAccion;
            _info_SGA = info_SGA;

            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InvestigacionIdRouteValue);
        }

        public async Task<IActionResult> ImplementarAccion(string IdAccion)
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
                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdAccion));

                var accion = await _getAccioIncidenteInvestigacion.Execute(IdDesencriptado);
                if (accion.Estado.Equals(EstadosIncidentes.Registrado.ToString()) || 
                    accion.Estado.Equals(EstadosIncidentes.Rechazado.ToString()) ||
                    accion.Estado.Equals(EstadosIncidentes.Ampliado.ToString()))
                {
                    var model = new ImplementarAccionModel();
                    model.Id_Accion_Incidente_Investigacion = accion.Id_Accion_Correctiva_Incidente;
                    model.Id_Encriptado = IdAccion;
                    model.Id_Investigacion_Encriptado = _protector.Protect(accion.Id_Investigacion_Final.ToString());
                    model.Id_Investigacion_Final = accion.Id_Investigacion_Final;
                    model.Tipo_Accion = accion.Tipo_Accion;
                    model.Accion_Inmediata = accion.Accion_Inmediata;
                    model.Responsable_Marcobre = accion.Responsable_Marcobre;
                    model.Responsable_Contratista = accion.Responsable_Contratista;
                    model.Fecha = accion.Fecha;
                    model.Estatus = accion.Estatus;
                    model.Estado = accion.Estado;

                    return View(model);
                }
                else {
                    var IdEncriptado = _protector.Protect(accion.Id_Investigacion_Final.ToString());
                    return RedirectToAction("DetalleInvestigacion", "InvestigacionFinal", new { IdInvestigacion = IdEncriptado });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> ImplementarAccion(ImplementarAccionModel model)
        {
            if (ModelState.IsValid)
            {
                var evidencias = new List<string>();
                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias[i].FileName);
                        var max_file_size = 8388608;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPGE", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i+1) + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                            evidencias.Add(filePath);
                        }
                    }
                }
                await _implementarAccion.Execute(model.Id_Accion_Incidente_Investigacion, evidencias, model.Comentarios_Implementacion);
                return RedirectToAction("NotificarAccionImplementada", "NotificacionInvestigacionIncidente", new { IdAccion = model.Id_Accion_Incidente_Investigacion });
            }
            return View(model);
        }
        //Carga y descarga de archivos
        private string CreateUniqueFileName(int index)
        {
            return "Ev_Inc_Inv_Acc" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")+"_"+index;
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
        public async Task<IActionResult> Download(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre-Prueba\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = "desencriptado.zip";
                    await DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
                    /*read decrypted file*/
                    FileStream stream = new FileStream(archivePath + decryptedFileName, FileMode.Open);

                    return File(stream, GetContentType(decryptedFileName), fileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }
        public async Task<string> UploadFTPFileAsync(string path, string newFileName)
        {
            /*if (!(HttpContext.Session.GetString("SessionUser") != null))
                return "NoAutorizado";
            else
            {*/
            Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvAccion;

            var serverInfo = await _info_SGA.Execute(directorio);

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
                using (FileStream fs = System.IO.File.OpenRead(path))
                {
                    sftp.UploadFile(fs, newFileName, true);
                }
                sftp.Dispose();
            }
            return newFileName;
            //}
        }
        public async Task<string> UploadZipFileAsync(IFormFile file, string newFileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;


                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre-Prueba/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/archive.zip";
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

                var fileName = filePath + file.FileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    //CopyToAsync corrompe los archivos
                    file.CopyTo(stream);
                    stream.Close();
                }
                ZipFile.CreateFromDirectory(filePath, archiveFile);

                var encryptedArchiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/encriptado.zip";
                await EncryptFileAsync(archiveFile, archivePath + "encriptado.zip");

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName);
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvAccion;

                var serverInfo = await _info_SGA.Execute(directorio);
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
                //ftpStream.CopyTo(fileStream);
                fileStream.Close();

                return downloadedFilePath;
            }
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
    }
}