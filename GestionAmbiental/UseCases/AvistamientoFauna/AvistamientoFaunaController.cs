using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientosFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.Items;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.RegistrarAvistamientoFauna;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using ImageMagick;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.AvistamientoFauna
{
    public class AvistamientoFaunaController : Controller
    {
        private readonly IGetItems _getItems;
        private readonly IGetAvistamientosFauna _getAvistamientosFauna;

        private readonly IGetUsuario _getUsuario;
        private readonly IRegistrarAvistamientoFauna _registrarAvistamientoFauna;

        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        public AvistamientoFaunaController(
            IGetItems getItems, 
            IGetAvistamientosFauna getAvistamientosFauna,
            IGetUsuario getUsuario,
            IGetInfo_SGA info_SGA,
            IRegistrarAvistamientoFauna registrarAvistamientoFauna,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getItems = getItems;
            _getAvistamientosFauna = getAvistamientosFauna;
            _getUsuario = getUsuario;
            _registrarAvistamientoFauna = registrarAvistamientoFauna;
            _info_SGA = info_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.AvistamientoFaunaIdRouteValue);
        }

        public async Task<IActionResult> Resumen()
        {
            var items = await _getItems.Execute();
            
            var tiposFauna = items.TiposFauna.Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
            tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var estadosFauna = items.EstadosFauna.Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
            estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.TipoFauna = tiposFauna;
            ViewBag.EstadoFauna = estadosFauna;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha, int tipo_fauna, int estado_fauna, int rpp, int pagina)
        {
            var datosTabla = await _getAvistamientosFauna.Execute(fecha,tipo_fauna,estado_fauna,rpp,pagina);

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NAvistamientos % rpp != 0 ? (datosTabla.NAvistamientos / rpp) + 1 : datosTabla.NAvistamientos / rpp;
            if (datosTabla.NAvistamientos > 0)
            {
                foreach (var fila in datosTabla.Avistamientos)
                {
                    //var IdEncriptado = _protector.Protect(fila.Id_Investigacion_Final.ToString());
                    
                    filasTabla += "<tr>";
                    if (fila.Estado.Equals(EstadosAvistamiento.Registrado.ToString()))
                        filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                    else
                        filasTabla += "<td></td>";

                    var IdEncriptado = _protector.Protect(fila.IdAvistamientoFauna.ToString());
                    filasTabla += "<td><a href=\"/AvistamientoFauna/EditarAvistamiento?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.Fecha_Avistamiento.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.Fecha_Avistamiento.ToString("HH:mm") + "</td>";
                    filasTabla += "<td>" + fila.Ubicacion + "</td>";
                    filasTabla += "<td>" + fila.Cantidad + "</td>";
                    filasTabla += "<td>" + fila.TipoFauna + "</td>"; 
                    filasTabla += "<td>" + fila.NombreComun + "</td>"; 
                    filasTabla += "<td>" + fila.EstadoFauna + "</td>"; 
                    filasTabla += "<td>" + fila.TipoAvistamiento + "</td>"; 
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
        public async Task<IActionResult> CatalogoFauna()
        {

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CargarCatalogo()
        {
            var items = await _getItems.Execute();
            var tiposFauna = items.TiposFauna;
            var nombresComunes = items.NombresComunes;

            var catalogo = "";
            foreach (var tipoFauna in tiposFauna)
            {
                catalogo += "<div class=\"card\">";
                catalogo += "<div class=\"card-header\">";
                catalogo += "<h5 class=\"mb-0 text-center\">";
                catalogo += "<button class=\"btn btn-link collapsed text-dark font-weight-bold\" data-toggle=\"collapse\" data-target=\"#collapse" + tipoFauna.Descripcion + "\" aria-expanded=\"true\" aria-controls=\"collapse" + tipoFauna.Descripcion + "\">" + tipoFauna.Descripcion + "</button>";
                catalogo += "</h5>";
                catalogo += "</div>";
                catalogo += "<div id=\"collapse" + tipoFauna.Descripcion + "\" class=\"collapse\" aria-labelledby=\"heading" + tipoFauna.Descripcion + "\" data-parent=\"#accordion\">";
                catalogo += "<div class=\"card-body\">";
                catalogo += "<div class=\"row text-center text-lg-left\">";

                var especiesPorTipo = nombresComunes.Where(x => x.TipoFauna.IdTipoFauna.Equals(tipoFauna.IdTipoFauna)).ToList();
                foreach (var nombreComun in especiesPorTipo)
                {
                    catalogo += "<div class=\"col-lg-3 col-md-4 col-6 mt-2\">";
                    catalogo += "<div class=\"card\" style=\"width:18rem;\">";
                    catalogo += "<img src=\""+nombreComun.Foto+ "\" style=\"width:100%; height:18rem;\" class=\"card-img-top img-thumbnail\" alt=\"...\">";
                    catalogo += "<div class=\"card-body\">";
                    catalogo += "<p class=\"card-text text-center\">" + nombreComun.Descripcion+"</p>";
                    catalogo += "</div>";
                    catalogo += "</div>";
                    catalogo += "</div>";
                }
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "</div>";
                catalogo += "</div>";
            }
            dynamic response = new
            {
                catalogo = catalogo
            };
            return Ok(response);
        }
        public async Task<IActionResult> NuevoRegistro()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var usuario = await _getUsuario.Execute(userInfo.NombreUsuario);

                ViewBag.Usuario = usuario.Nombres + " " + usuario.Apellidos;

                var items = await _getItems.Execute();

                var tiposFauna = items.TiposFauna.Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosFauna = items.EstadosFauna.Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoAvistamiento = items.TiposAvistamiento.Select(g => new SelectListItem() { Value = g.IdTipoAvistamiento.ToString(), Text = g.Descripcion }).ToList();
                tipoAvistamiento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresas = items.Empresas.Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var areas = items.AreasTrabajo.Select(g => new SelectListItem() { Value = g.IdAreaTrabajo.ToString(), Text = g.Descripcion }).ToList();
                areas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nombresComunes = new List<SelectListItem>();
                foreach (var grupo in items.TiposFauna)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Descripcion, Disabled = false };
                    nombresComunes.AddRange(items.NombresComunes.Where(y => y.TipoFauna.IdTipoFauna.Equals(grupo.IdTipoFauna)).OrderBy(x => x.Descripcion).Select(i => new SelectListItem() { Value = i.IdNombreComunFauna.ToString(), Text = i.Descripcion, Group = nuevoGrupo }).ToList());
                }
                nombresComunes.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.TipoFauna = tiposFauna;
                ViewBag.EstadoFauna = estadosFauna;
                ViewBag.NombreComun = nombresComunes;
                ViewBag.TipoAvistamiento = tipoAvistamiento;
                ViewBag.Empresa = empresas;
                ViewBag.AreaTrabajo = areas;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(AvistamientoFaunaModel model)
        {
            if (ModelState.IsValid)
            {
                var fotos = new List<string>();
                foreach (var foto in model.Fotos)
                {
                    var filePath = "";

                    var extension = Path.GetExtension(foto.FileName);
                    var max_file_size = 8388608;

                    var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPGE", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                    if (allowed_extensions.Contains(extension) && foto.Length < max_file_size)
                    {
                        var newfileName = CreateUniqueFileName(1) + extension;
                        filePath = await CargarImagen(foto, newfileName);
                        fotos.Add(filePath);                   
                    }
                }
                await _registrarAvistamientoFauna.Execute(fotos.ToArray(),
                    model.Fecha_Avistamiento,
                    model.Hora_Avistamiento,
                    model.Ubicacion,
                    model.Cantidad,
                    model.IdNombreComun,
                    model.IdTipoAvistamiento,
                    model.IdEstadoFauna,
                    model.Lugar_Referencia,
                    278,
                    model.IdEmpresa,
                    model.IdAreaTrabajo,
                    model.NFotocheck);
            }
            return RedirectToAction("NuevoRegistro");
        }
        public async Task<IActionResult> ObtenerImagenReferencia(int IdNombreComun)
        {
            var items = await _getItems.Execute();
            var nombreComun = items.NombresComunes.FirstOrDefault(x => x.IdNombreComunFauna.Equals(IdNombreComun));
            dynamic response = new
            {
                Id = nombreComun.IdNombreComunFauna,
                Nombre = nombreComun.Descripcion,
                Foto = nombreComun.Foto
            };
            return Ok(response);
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia_Avistamiento_Fauna_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return fileName;
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private async Task<string> CargarImagen(IFormFile file, string newFileName)
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

                var fileName = filePath + file.FileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    file.CopyTo(stream);

                    ImageOptimizer optimizer = new ImageOptimizer();

                    // Set the stream position to the beginning of the file.
                    stream.Seek(0, SeekOrigin.Begin);
                    try
                    {
                        optimizer.LosslessCompress(stream);
                    }catch(Exception e) { }
                    stream.Flush();
                                       
                    stream.Close();
                }
                var encryptedArchiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/"+newFileName;
                await EncryptFileAsync(fileName, encryptedArchiveFile);

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName);
            }
            else
                return "Error";         
        }
        public async Task<string> UploadFTPFileAsync(string path, string newFileName)
        {
            if (HttpContext.Session.GetString("SessionUser") != null)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaRACR);
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
            }
            else
            {
                return "No Autorizado";
            }
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
        public async Task<string> DownloadFTPFileAsync(string fileName)
        {
            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaRACR);
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