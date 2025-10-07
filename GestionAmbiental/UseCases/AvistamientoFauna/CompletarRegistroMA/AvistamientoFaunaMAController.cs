using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.CompletarAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.EditarAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientosFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.Items;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Util.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.AvistamientoFauna.CompletarRegistroMA
{
    public class AvistamientoFaunaMAController : Controller
    {
        private readonly IGetItems _getItems;
        private readonly IGetAvistamientosFauna _getAvistamientosFauna;
        private readonly IGetAvistamientoFauna _getAvistamientoFauna;

        private readonly IGetUsuario _getUsuario;
        private readonly IEditarAvistamientoFauna _editarAvistamientoFauna;
        private readonly ICompletarAvistamientoFauna _completarAvistamientoFauna;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        public AvistamientoFaunaMAController(
            IGetItems getItems,
            IGetAvistamientosFauna getAvistamientosFauna,
            IGetAvistamientoFauna getAvistamientoFauna,
            IGetUsuario getUsuario,
            IGetInfo_SGA info_SGA,
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IDataProtectionProvider dataProtectionProvider,
            IEditarAvistamientoFauna editarAvistamientoFauna,
            ICompletarAvistamientoFauna completarAvistamientoFauna,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getItems = getItems;
            _getAvistamientosFauna = getAvistamientosFauna;
            _getAvistamientoFauna = getAvistamientoFauna;
            _getUsuario = getUsuario;
            _editarAvistamientoFauna = editarAvistamientoFauna;
            _completarAvistamientoFauna = completarAvistamientoFauna;
            _info_SGA = info_SGA;
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.AvistamientoFaunaIdRouteValue);
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

                var items = await _getItems.Execute();

                var tiposFauna = items.TiposFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosFauna = items.EstadosFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
                estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugaresReferencia = items.LugaresReferencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();
                lugaresReferencia.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var contactosEmergencia = JsonConvert.SerializeObject(items.ContactosEmergencia.Select(x => x.Telefono).ToArray());

                ViewBag.Contactos = contactosEmergencia;
                ViewBag.LugarReferencia = lugaresReferencia;
                ViewBag.TipoFauna = tiposFauna;
                ViewBag.EstadoFauna = estadosFauna;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha, int tipo_fauna, int estado_fauna, int lugar_referencia, int rpp, int pagina)
        {
            var datosTabla = await _getAvistamientosFauna.Execute(fecha, tipo_fauna, estado_fauna, lugar_referencia, rpp, pagina);
            //Solo los no vivos (IdEstadoFauna != 1)
            datosTabla.Avistamientos = datosTabla.Avistamientos.Where(x => !x.IdEstadoFauna.Equals(1) && !(x.Estado.Equals(EstadosAvistamiento.Pendiente.ToString())|| x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()))).ToList();

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NAvistamientos % rpp != 0 ? (datosTabla.NAvistamientos / rpp) + 1 : datosTabla.NAvistamientos / rpp;
            if (datosTabla.Avistamientos.Count > 0)
            {
                foreach (var fila in datosTabla.Avistamientos)
                {
                    if (!fila.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()))
                    {
                        var IdEncriptado = _protector.Protect(fila.IdAvistamientoFauna.ToString());
                        filasTabla += "<tr>";
                        if (fila.Estado.Equals(EstadosAvistamiento.Registrado.ToString()))
                        {
                            filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                            filasTabla += "<td><a href=\"/AvistamientoFaunaMA/CompletarRegistro?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                        }
                        else if (fila.Estado.Equals(EstadosAvistamiento.Completado.ToString()))
                        {
                            filasTabla += "<td></td>";
                            filasTabla += "<td><a href=\"/AvistamientoFauna/DetalleRegistro?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                        }
                        filasTabla += "<td>" + fila.FechaAvistamiento.ToString("dd/MM/yyyy") + "</td>";
                        filasTabla += "<td>" + fila.FechaAvistamiento.ToString("HH:mm") + "</td>";
                        filasTabla += "<td>" + fila.Ubicacion + "</td>";
                        filasTabla += "<td>" + fila.Cantidad + "</td>";
                        filasTabla += "<td>" + fila.TipoFauna + "</td>";
                        filasTabla += "<td>" + fila.NombreComun + "</td>";
                        filasTabla += "<td>" + fila.EstadoFauna + "</td>";
                        filasTabla += "<td>" + fila.TipoAvistamiento + "</td>";
                        filasTabla += "</tr>";
                    }
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
                var nCol = 10;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"" + nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> CompletarRegistro(string IdAvistamiento)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAvistamiento=" + IdAvistamiento });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var usuario = await _getUsuario.Execute(userInfo.NombreUsuario);

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var idAvistamiento = Int32.Parse(_protector.Unprotect(IdAvistamiento));
                var avistamiento = await _getAvistamientoFauna.Execute(idAvistamiento);

                var items = await _getItems.Execute();

                var tiposFauna = items.TiposFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();

                var estadosFauna = items.EstadosFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();

                var tipoAvistamiento = items.TiposAvistamiento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoAvistamiento.ToString(), Text = g.Descripcion }).ToList();

                var empresas = items.Empresas.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var areas = items.AreasTrabajo.Where(x=>x.Estado.Equals("ACT")).OrderBy(x=>x.Descripcion).Select(g => new SelectListItem() { Value = g.IdAreaTrabajo.ToString(), Text = g.Descripcion }).ToList();
                areas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugares = items.LugaresReferencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdLugarReferencia.ToString(), Text = g.Descripcion }).ToList();

                var nombresComunes = new List<SelectListItem>();
                foreach (var grupo in items.TiposFauna)
                {
                    var nuevoGrupo = new SelectListGroup() { Name = grupo.Descripcion, Disabled = false };
                    nombresComunes.AddRange(items.NombresComunes.Where(y => y.TipoFauna.IdTipoFauna.Equals(grupo.IdTipoFauna)).OrderBy(x => x.Descripcion).Select(i => new SelectListItem() { Value = i.IdNombreComunFauna.ToString(), Text = i.Descripcion, Group = nuevoGrupo }).ToList());
                }

                var contactosEmergencia = JsonConvert.SerializeObject(items.ContactosEmergencia.Select(x => x.Telefono).ToArray());

                ViewBag.Contactos = contactosEmergencia;
                ViewBag.TipoFauna = tiposFauna;
                ViewBag.EstadoFauna = estadosFauna;
                ViewBag.NombreComun = nombresComunes;
                ViewBag.TipoAvistamiento = tipoAvistamiento;
                ViewBag.Empresa = empresas;
                ViewBag.AreaTrabajo = areas;
                ViewBag.LugarReferencia = lugares;

                var model = new AvistamientoFaunaModel();
                model.IdAvistamientoFauna = avistamiento.IdAvistamientoFauna;
                model.Fecha_Avistamiento = avistamiento.FechaAvistamiento.ToString("yyyy-MM-dd");
                model.Hora_Avistamiento = avistamiento.FechaAvistamiento.ToString("HH:mm");
                model.Ubicacion = avistamiento.Ubicacion;
                model.Cantidad = avistamiento.Cantidad;
                model.IdNombreComun = avistamiento.IdNombreComun;
                model.IdTipoAvistamiento = avistamiento.IdTipoAvistamiento;
                model.IdEstadoFauna = avistamiento.IdEstadoFauna;
                model.IdLugarReferencia = avistamiento.IdLugarReferencia;

                model.IdUsuario = avistamiento.IdUsuario;
                model.Nombres_Apellidos = avistamiento.Usuario;
                if (avistamiento.IdEmpresa != null)
                    model.IdEmpresa = avistamiento.IdEmpresa.Value;
                if (avistamiento.IdAreaTrabajo != null)
                    model.IdAreaTrabajo = avistamiento.IdAreaTrabajo.Value;
                model.DNI = avistamiento.DNI;

                model.FotosCargadas = avistamiento.Fotos != null ? JsonConvert.SerializeObject(avistamiento.Fotos) : null;
                model.Estado = avistamiento.Estado;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CompletarRegistro(AvistamientoFaunaModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var evidencias = new List<string>();
                if (model.Evidencia_Accion != null)
                {
                    for (int i = 0; i < model.Evidencia_Accion.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencia_Accion[i].FileName);
                        var max_file_size = 8388608;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencia_Accion[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencia_Accion[i], newfileName);
                            evidencias.Add(filePath);
                        }
                    }                    
                }
                var fotos = new List<string>();

                if (model.FotosCargadas != null)
                {
                    var listaImagenes = new List<string>(model.FotosCargadas.Split(','));
                    fotos.AddRange(listaImagenes);
                    var vacios = new List<int>();
                    foreach (var foto in fotos)
                    {
                        if (foto.Length == 0)
                            vacios.Add(fotos.IndexOf(foto));
                    }
                    foreach (var vacio in vacios)
                    {
                        fotos.RemoveAt(vacio);
                    }
                }

                await _completarAvistamientoFauna.Execute(model.IdAvistamientoFauna,
                    fotos.ToArray(),
                    model.Fecha_Avistamiento,
                    model.Hora_Avistamiento,
                    model.Ubicacion,
                    model.Cantidad,
                    model.IdNombreComun,
                    model.IdTipoAvistamiento,
                    model.IdEstadoFauna,
                    model.IdLugarReferencia,
                    model.IdUsuario,
                    model.IdEmpresa,
                    model.IdAreaTrabajo,
                    model.DNI,
                    evidencias.ToArray(),
                    model.Descripcion_Accion,
                    userInfo.NombreUsuario);
            }

            return RedirectToAction("NotificarAccionAvistamiento", "NotificacionesAvistamientoFauna", new { IdAvistamiento = _protector.Protect(model.IdAvistamientoFauna.ToString())});
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia_Accion_Avistamiento_Fauna_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?fileName=" + fileName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                try
                {
                    var encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(fileName,userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionAvistamientoFaunaAccion);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "Descarga Temporal Gestiona\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = "desencriptado.zip";
                    await _gestorEncriptado.DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
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
        public async Task<string> UploadZipFileAsync(IFormFile file, string newFileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return "NoAutorizado";
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "Descarga Temporal Gestiona/";
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
                await _gestorEncriptado.EncryptFileAsync(archiveFile, archivePath + "encriptado.zip");

                return await _gestorArchivos.UploadFTPFileAsync(encryptedArchiveFile, newFileName, Tipo_Info_SGA.FTPConnectionAvistamientoFaunaAccion);
            }
        }
        
    }
}
