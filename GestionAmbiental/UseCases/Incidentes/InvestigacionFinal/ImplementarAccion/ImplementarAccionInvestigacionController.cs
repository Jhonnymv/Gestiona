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
using GestionAmbiental.UseCases.Util.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.Incidentes.InvestigacionFinal.ImplementarAccion
{
    public class ImplementarAccionInvestigacionController : Controller
    {
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        private readonly IGetAccioIncidenteInvestigacion _getAccioIncidenteInvestigacion;
        private readonly IImplementarAccion _implementarAccion;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;

        public ImplementarAccionInvestigacionController(
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IGetAccioIncidenteInvestigacion getAccioIncidenteInvestigacion,
            IImplementarAccion implementarAccion,
            IGetInfo_SGA info_SGA,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _getAccioIncidenteInvestigacion = getAccioIncidenteInvestigacion;
            _implementarAccion = implementarAccion;
            _info_SGA = info_SGA;

            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InvestigacionIdRouteValue);
        }
        public IActionResult ImplementarAccionNotificacion(int IdAccion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAccion=" + IdAccion });
            }
            else
            {
                return RedirectToAction("ImplementarAccion", new { IdAccion = _protector.Protect(IdAccion.ToString()) });
            }
        }
        public async Task<IActionResult> ImplementarAccion(string IdAccion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAccion=" + IdAccion });
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
                else
                {
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

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
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
            return "Ev_Inc_Inv_Acc" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + index;
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?fileName=" + fileName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                try
                {
                    var encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(fileName, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaIncInvAccion);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "Descarga Temporal Gestiona V2\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = archivePath + "desencriptado.zip";
                    await _gestorEncriptado.DecryptFileAsync(encryptedFile, decryptedFileName);
                    /*read decrypted file*/
                    FileStream stream = new FileStream(decryptedFileName, FileMode.Open);

                    return File(stream, GetContentType(decryptedFileName), fileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
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
                var SGATempPath = tempPath + "Descarga Temporal Gestiona V2/";
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

                var encryptedArchiveFile = archivePath + "/encriptado.zip";
                await _gestorEncriptado.EncryptFileAsync(archiveFile, encryptedArchiveFile);

                return await _gestorArchivos.UploadFTPFileAsync(encryptedArchiveFile, newFileName, Tipo_Info_SGA.FTPConnectionEvidenciaIncInvAccion);
            }
        }
    }
}