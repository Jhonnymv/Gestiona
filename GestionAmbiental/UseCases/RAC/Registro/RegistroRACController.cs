using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.RAC.Registro;
using GestionAmbiental.Application.UseCases.RAC.Registro.AmpliarFechaRAC;
using GestionAmbiental.Application.UseCases.RAC.Registro.ConsultarRAC;
using GestionAmbiental.Application.UseCases.RAC.Registro.ConsultarRACs;
using GestionAmbiental.Application.UseCases.RAC.Registro.ConsultarRACs;
using GestionAmbiental.Application.UseCases.RAC.Registro.RegistrarRAC;
using GestionAmbiental.Application.UseCases.RAC.Registro.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.RAC.Registro
{
    public class RegistroRACController : Controller
    {
        private readonly IConsultarRACs _consultarRACs;
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarRAC _registrarRAC;
        private readonly IConsultarRAC _consultarRAC;
        private readonly IGetUsuario _getUsuario;
        private readonly IAmpliarFechaRAC _ampliarFechaRAC;

        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        public RegistroRACController(
            IConsultarRACs consultarRACs,
            IConsultarRAC consultarRAC,
            ISelectItems selectItems,
            IRegistrarRAC registrarRAC,
            IGetUsuario getUsuario,
            IAmpliarFechaRAC ampliarFechaRAC,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _consultarRACs = consultarRACs;
            _consultarRAC = consultarRAC;
            _selectItems = selectItems;
            _registrarRAC = registrarRAC;
            _getUsuario = getUsuario;
            _ampliarFechaRAC = ampliarFechaRAC;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.RACIdRouteValue);
            _info_SGA = info_SGA;
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
                var usuarios = items.ItemsUsuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                ViewBag.Usuarios = usuarios;
                return View();
            }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha,int filtroUsuario, string filtroCodigo, int pagina, int rpp)
        {

            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarRACs.Execute(pagina, rpp, fecha, filtroCodigo,filtroUsuario);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Registros)
                {
                    var IdEncriptado = _protector.Protect(fila.Id_RAC.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><label style=\"width:80px;\"><a href=\"/RegistroRAC/DetalleRAC?IdRAC="+IdEncriptado+"\">" + fila.Codigo + "</a></label></td>";
                    filasTabla += "<td>" + fila.Nombres_Apellidos + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.Empresa_Ob + "</td>";
                    filasTabla += "<td>" + fila.Gerencia_Obs + "</td>";
                    filasTabla += "<td>" + fila.SSO_MA + "</td>";
                    filasTabla += "<td>" + fila.Clasificacion + "</td>";
                    switch (fila.NivelRiesgo)
                    {
                        case "Tolerable": filasTabla += "<td><span class=\"badge badge-success\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Alerta": filasTabla += "<td><span class=\"badge badge-warning\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "No Aceptable": filasTabla += "<td><span class=\"badge badge-danger\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Bajo": filasTabla += "<td><span class=\"badge badge-success\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Medio": filasTabla += "<td><span class=\"badge badge-warning\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Alto": filasTabla += "<td><span class=\"badge badge-danger\">" + fila.NivelRiesgo + "</span></td>"; break;
                        default: filasTabla += "<td><span class=\"badge badge-secondary\">" + fila.NivelRiesgo + "</span>"; break;
                    }
                    filasTabla += "<td>" + fila.TipoCondicion + "</td>";
                    filasTabla += "<td>" + fila.Fecha.ToString("dd/MM/yyyy HH:mm") + "</td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                }
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
        public async Task<IActionResult> NuevoRAC()
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
                var usuario = await _getUsuario.Execute(userInfo.NombreUsuario);

                var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresas = items.ItemsEmpresa.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoMA = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionMA = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoSSO = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoSSO.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionSSO = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionSSO.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresMAP = items.ItemsAsesoresMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresMAP.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresSSO = items.ItemsAsesoresSSO.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresSSO.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Gerencias = gerencias;
                ViewBag.Empresas = empresas;
                ViewBag.Usuarios = usuarios;
                ViewBag.AsesoresMAP = asesoresMAP;
                ViewBag.AsesoresSSO = asesoresSSO;

                ViewBag.NombreUsuario = usuario.Nombres + " " + usuario.Apellidos;

                ViewBag.ActoMA = tiposActoMA;
                ViewBag.CondicionMA = tiposCondicionMA;
                ViewBag.ActoSSO = tiposActoSSO;
                ViewBag.CondicionSSO = tiposCondicionSSO;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRAC(NuevoRACModel model)
        {
            if (ModelState.IsValid)
            {
                if (!(HttpContext.Session.GetString("SessionUser") != null))
                {
                    return RedirectToAction("NoAutorizado", "Home");
                }
                else
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                    var filePath = "";
                    if (model.Evidencia_Obs != null)
                    {
                        var newfileName = CreateUniqueFileName(1) + ".zip";
                        filePath = await UploadZipFileAsync(model.Evidencia_Obs, newfileName);
                    }
                    var Id_RAC = await _registrarRAC.Execute(
                        model.Nombres_Apellidos,
                        model.Id_Gerencia,
                        model.Id_Empresa,
                        model.Otra_Empresa,
                        model.Id_Empresa_Obs,
                        model.Id_Gerencia_Obs,
                        model.Ubicacion,
                        model.Fecha,
                        model.Hora,
                        model.SSO_MA,
                        model.Clasificacion,
                        model.NivelRiesgo,
                        model.Id_Tipo_Condicion,
                        model.Descripcion_Acto_Cond,
                        model.Medida_Control,
                        model.PAC,
                        model.Id_Responsable_Cierre,
                        model.Fecha_Cierre,
                        model.Id_Supervisor,
                        filePath,
                        userInfo.NombreUsuario);
                    //Notificar
                    return RedirectToAction("NotificarNuevoRAC", "Notificaciones", new { IdRAC = Id_RAC });
                }
            }
            else
                return RedirectToAction("NuevoRAC");
        }
        public async Task<IActionResult> DetalleRAC(string IdRAC)
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
                var IdDescencriptado = Int32.Parse(_protector.Unprotect(IdRAC));
                var reporte = await _consultarRAC.Execute(IdDescencriptado);
                var model = new DetallerRACModel
                {
                    Id_RAC = reporte.Id_RAC,
                    IdEncriptado = IdRAC,
                    Nombres_Apellidos = reporte.Nombres_Apellidos,
                    Gerencia = reporte.Gerencia,
                    Empresa = reporte.Empresa,
                    Otra_Empresa = reporte.Otra_Empresa,
                    Empresa_Obs = reporte.Empresa_Ob,
                    Gerencia_Obs = reporte.Gerencia_Obs,
                    Ubicacion = reporte.Ubicacion,
                    Fecha = reporte.Fecha.ToString("dd/MM/yyyy"),
                    Hora = reporte.Fecha.ToString("HH:mm"),
                    SSO_MA = reporte.SSO_MA,
                    Clasificacion = reporte.Clasificacion,
                    NivelRiesgo = reporte.NivelRiesgo,
                    Tipo_Condicion = reporte.TipoCondicion,
                    Descripcion_Acto_Cond = reporte.Descripcion_Acto_Cond,
                    Medida_Control = reporte.Medida_Control,
                    PAC = reporte.PAC,
                    Responsable_Cierre = reporte.Responsable_Cierre,
                    Fecha_Cierre = reporte.Fecha_Cierre.ToString("dd/MM/yyyy"),
                    Supervisor = reporte.Supervisor,
                    Evidencia_Obs = reporte.Evidencia_Obs,
                    Estado = reporte.Estado                   
                };
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AmpliarFecha(string idRAC, string fecha)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var IdDescencriptado = Int32.Parse(_protector.Unprotect(idRAC));
            await _ampliarFechaRAC.Execute(IdDescencriptado, fecha, userInfo.NombreUsuario);
            return RedirectToAction("DetalleRAC", new { IdRAC = idRAC });
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia__Registro_RAC_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return fileName;
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
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName);

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
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaRACR);
                string route = "", user = "", pass = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "route": route = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                    }
                }
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(route + newFileName);

                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                using (FileStream fs = System.IO.File.OpenRead(path))
                {

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    Stream requestStream = ftpRequest.GetRequestStream();
                    await requestStream.WriteAsync(buffer, 0, buffer.Length);
                    await requestStream.FlushAsync();
                    requestStream.Close();
                }
                return newFileName;
            }
            else { 
                return "NoAutorizado"; 
            }
        }
        public async Task<string> UploadZipFileAsync(IFormFile file, string newFileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "No autorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
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
                //await DecryptFileAsync(archivePath + "encriptado.zip", archivePath + "desencriptado.zip");

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName);
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName)
        {
            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                
                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaRACR);
                string route = "", user = "", pass = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "route": route = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                    }
                }
                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(route + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                var ftpStream = ftpResponse.GetResponseStream();

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
                /*New file downloaded name*/
                var downloadedFilePath = archivePath + fileName;
                /*Copy data to new temp file*/
                var fileStream = System.IO.File.Create(downloadedFilePath);
                ftpStream.CopyTo(fileStream);
                fileStream.Close();

                return downloadedFilePath;
            }
            catch (Exception e)
            {
                return e.Message;
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