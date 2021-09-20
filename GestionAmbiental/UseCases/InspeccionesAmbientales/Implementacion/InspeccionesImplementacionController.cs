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
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Inspecciones.AmpliarFechaObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarDetalles;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ImplementarObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Implementacion
{
    public class InspeccionesImplementacionController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IConsultarCabecerasImplementacion _consultarCabeceras;
        private readonly IConsultarCabeceraImplementacion _consultarCabecera;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IConsultarDetalleImplementacion _consultarDetalleImplementacion;
        private readonly IImplementarObservacion _implementarObservacion;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public InspeccionesImplementacionController(
            ISelectItems selectItems,
            IConsultarCabecerasImplementacion consultarCabeceras,
            IConsultarCabeceraImplementacion consultarCabecera,
            IConsultarInspeccionDetalle consultarInspeccionDetalle,
            IConsultarDetalleImplementacion consultarDetalleImplementacion,
            IImplementarObservacion implementarObservacion,
            IWebHostEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA
            )
        {
            _selectItems = selectItems;
            _consultarCabeceras = consultarCabeceras;
            _consultarCabecera = consultarCabecera;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarDetalleImplementacion = consultarDetalleImplementacion;
            _implementarObservacion = implementarObservacion;
            _hostingEnvironment = hostingEnvironment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
            _info_SGA = info_SGA;
        }
        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var selectItemsResumen = await _selectItems.ItemVistaResumen();

                var filtroEmpresas = selectItemsResumen.EmpContratMarcobre.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                var filtroGerencias = selectItemsResumen.GerenciaEjecutora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                var filtroUsuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();

                var usuariosAgrupados = selectItemsResumen.Usuarios.GroupBy(x => x.IdRol).ToList();
                var asesores = new List<SelectListItem>();
                foreach (var grupo in usuariosAgrupados)
                {
                    if (grupo.Key.Equals((int)InspeccionRol.AsesorMAP))
                    {
                        var nuevoGrupo = new SelectListGroup() { Name = "Asesores MAP", Disabled = false };
                        asesores.AddRange(grupo.OrderBy(x => x.Nombres).Select(i => new SelectListItem() { Value = i.IdUsuario.ToString(), Text = i.Nombres + " " + i.Apellidos, Group = nuevoGrupo }).ToList());
                    }
                    if (grupo.Key.Equals((int)InspeccionRol.AsesorSSO))
                    {
                        var nuevoGrupo = new SelectListGroup() { Name = "Asesores SSO", Disabled = false };
                        asesores.AddRange(grupo.OrderBy(x => x.Nombres).Select(i => new SelectListItem() { Value = i.IdUsuario.ToString(), Text = i.Nombres + " " + i.Apellidos, Group = nuevoGrupo }).ToList());
                    }
                }

                ViewBag.Empresas = filtroEmpresas;
                ViewBag.Gerencias = filtroGerencias;
                ViewBag.Usuarios = filtroUsuarios;
                ViewBag.Asesores = asesores;

                return View();
            }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string fechaInicioFiltro, string fechaFinFiltro, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigoFiltro, int pagina, int rpp)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            DateTime fechaInicio;
            DateTime fechaFin;
            if (fechaInicioFiltro != null)
                fechaInicio = Convert.ToDateTime(fechaInicioFiltro);
            else
                fechaInicio = (DateTime)SqlDateTime.MinValue;
            if (fechaFinFiltro != null)
                fechaFin = Convert.ToDateTime(fechaFinFiltro);
            else
                fechaFin = DateTime.Now;
            var datosTabla = await _consultarCabeceras.Execute(pagina, rpp, fechaInicio, fechaFin, gerenciaFiltro, empresaFiltro, registradoPorFiltro, responsableFiltro, asesorFiltro, codigoFiltro);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NCabeceras % rpp != 0 ? (datosTabla.NCabeceras / rpp) + 1 : datosTabla.NCabeceras / rpp;
            if (datosTabla.NCabeceras > 0)
            {
                foreach (var fila in datosTabla.Cabeceras)
                {

                    var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";                    
                    filasTabla += "<td><a href=\"/InspeccionesImplementacion/ImplementarInspeccion?IdCabecera=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.Gerencia + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.GerenciaInspeccionada + "</td>";
                    filasTabla += "<td>" + fila.AreaInspeccionada + "</td>";
                    filasTabla += "<td>" + fila.FechaInspeccion.ToString("dd/MM/yyyy HH:mm") + "</td>";
                    filasTabla += "<td>" + fila.NObservaciones + "</td>";
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
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"100%\"\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> ImplementarInspeccion(string IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdCabecera=" + IdCabecera });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));



                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (IdCabecera.Length != 0)
                {
                    int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdCabecera));
                    var cabecera = await _consultarCabecera.Execute(IdDesencriptado);
                    ViewBag.Cabecera = cabecera;
                    ViewBag.Codigo = cabecera.Codigo;
                    ViewBag.idCabecera = cabecera.IdCabecera;
                    var filaObservacion = "";
                    var observaciones = await _consultarInspeccionDetalle.Execute(IdDesencriptado, userInfo.NombreUsuario);
                    foreach (var observacion in observaciones)
                    {
                        var evidencia = "";
                        var evidencia_implementacion = "";
                        for (int i = 0; i < observacion.EvidenciaCondicion.Count; i++)
                            evidencia = "<a href=\"/Inspecciones/Download?fileName=" + observacion.EvidenciaCondicion[i] + "\">Evidencia</a>";

                        if (observacion.EvidenciaImplementacion != null)
                        {
                            for (int i = 0; i < observacion.EvidenciaImplementacion.Count; i++)
                                evidencia_implementacion = "<a href=\"/InspeccionesImplementacion/Download?fileName=" + observacion.EvidenciaImplementacion[i] + "\">Evidencia</a>";
                        }
                        filaObservacion += "<tr>";

                        if ((observacion.Estado.Equals(EstadosInspeccion.Registrado.ToString()) || 
                            observacion.Estado.Equals(EstadosInspeccion.Ampliado.ToString()) || 
                            (observacion.Estado.Equals(EstadosInspeccion.Rechazado.ToString()) && !observacion.DescripcionCondicion.Equals("Buenas Prácticas"))) &&
                            observacion.PermisosImplementar)                        
                            filaObservacion += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"ImplementarObservacion(" + observacion.IdInspeccionDetalle + ")\">" + "Implementar" + "</button></td>";
                        
                        else if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) &&
                                observacion.PermisosImplementar)                        
                            filaObservacion += "<td><span class=\"badge bg-warning\">Vencido</span></td>";        
                        
                        else
                            filaObservacion += "<td></td>";        

                        filaObservacion += "<td style=\"display: none; \">" + observacion.IdInspeccionDetalle + "</td>";
                        filaObservacion += "<td>" + observacion.TipoInspeccion + "</td>";
                        filaObservacion += "<td>" + observacion.AsesorMASeguridad + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionCondicion + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionActoCondicion + "</td>";
                        filaObservacion += "<td>" + observacion.TipoCondicion + "</td>";
                        filaObservacion += "<td>" + evidencia + "</td>";
                        switch (observacion.NivelRiesgo)
                        {
                            case "Tolerable": filaObservacion += "<td><span class=\"badge bg-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alerta": filaObservacion += "<td><span class=\"badge bg-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "No Aceptable": filaObservacion += "<td><span class=\"badge bg-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Bajo": filaObservacion += "<td><span class=\"badge bg-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Medio": filaObservacion += "<td><span class=\"badge bg-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alto": filaObservacion += "<td><span class=\"badge bg-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            default: filaObservacion += "<td><span class=\"badge bg-secondary\">" + observacion.NivelRiesgo + "</span>"; break;
                        }
                        filaObservacion += "<td>" + observacion.DescripcionAccion + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableMarcobre + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableContratista + "</td>";
                        filaObservacion += "<td>" + observacion.CorreoContratista + "</td>";
                        filaObservacion += "<td>" + (observacion.FechaProgramada.HasValue ? observacion.FechaProgramada.Value.ToShortDateString() : "") + "</td>";
                        if ((observacion.Estado.Equals(EstadosInspeccion.Registrado.ToString()) || observacion.Estado.Equals(EstadosInspeccion.Ampliado.ToString())) &&
                            observacion.PermisosAsesor)
                        {
                            filaObservacion += "<td><span class=\"badge bg-secondary\">Pendiente</span></td>";
                        }
                        else if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) &&
                            observacion.PermisosAsesor)
                        {
                            filaObservacion += "<td><span class=\"badge bg-warning\">Vencido</span></td>";
                        }
                        else if (observacion.Estado.Equals(EstadosInspeccion.Aprobado.ToString()))
                            filaObservacion += "<td><span class=\"badge bg-success\">Aprobado</span></td>";
                        else if (observacion.Estado.Equals(EstadosInspeccion.Rechazado.ToString()))
                        {
                            filaObservacion += "<td><span class=\"badge bg-danger\">Rechazado</span></td>";
                        }
                        else if (observacion.Estado.Equals(EstadosInspeccion.Implementado.ToString()))
                        {
                            filaObservacion += "<td><span class=\"badge bg-info\">Implementado</span></td>";
                        }
                        else
                        {
                            filaObservacion += "<td><span class=\"badge bg-secondary\">Pendiente</span></td>";
                        }
                        filaObservacion += "</tr>";
                    }
                    var jsonObservaciones = JsonConvert.SerializeObject(observaciones.Select(x => new
                    {
                        IdObservacion = x.IdInspeccionDetalle,
                        EvidenciasCondicion = JsonConvert.SerializeObject(x.EvidenciaCondicion),
                        TipoInspeccion = x.TipoInspeccion,
                        Asesor = x.AsesorMASeguridad,
                        DescripcionActoCondicion = x.DescripcionActoCondicion,
                        DescripcionCondicion = x.DescripcionCondicion,
                        TipoCondicion = x.TipoCondicion,
                        NivelRiesgo = x.NivelRiesgo,
                        Accion = x.DescripcionAccion,
                        ResponsableMarc = x.ResponsableMarcobre,
                        ResponsableCont = x.ResponsableContratista,
                        FechaProgramada = x.FechaProgramada.HasValue ? x.FechaProgramada.Value.ToString("yyyy-MM-dd") : "",
                        Estado = x.Estado,
                        PermisoImplementar = x.PermisosImplementar
                    }));
                    ViewBag.Observaciones = jsonObservaciones;
                    ViewBag.ContenidoTabla = filaObservacion;
                    ViewBag.NObservaciones = observaciones.Count;
                }
                return View();
            }
        }
        public async Task<IActionResult> ImplementarObservacion(string IdObservacion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdObservacion=" + IdObservacion });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
                var observacion = await _consultarDetalleImplementacion.Execute(IdDesencriptado);
                var IdCabeceraEncriptado = _protector.Protect(observacion.IdInspeccionCabecera.ToString());
                var model = new LevantarObservacionModel();
                model.EncryptedId = IdObservacion;
                model.TipoInspeccion = observacion.TipoInspeccion;
                model.AsesorSeguridad = observacion.AsesorMASeguridad;
                model.idObservacion = observacion.IdInspeccionDetalle;
                model.idCabecera = observacion.IdInspeccionCabecera;
                model.CondicionIdentificada = observacion.DescripcionCondicion;
                model.TipoCondicion = observacion.TipoCondicion;
                model.Evidencia = observacion.EvidenciaCondicion;
                model.NivelRiesgo = observacion.NivelRiesgo;
                model.AccionTomada = observacion.DescripcionAccion;
                model.ResponsableMarc = observacion.ResponsableMarcobre;
                model.ResponsableCont = observacion.ResponsableContratista;
                model.FechaProgramada = observacion.FechaProgramada.HasValue ? observacion.FechaProgramada.Value.ToShortDateString() : "";

                ViewBag.IdCabecera = IdCabeceraEncriptado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ImplementarObservacion(LevantarObservacionModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var evidencias = new List<string>();
                if (model.EvidenciasInspeccion != null)
                {
                    for (int i = 0; i < model.EvidenciasInspeccion.Count; i++)
                    {
                        var extension = Path.GetExtension(model.EvidenciasInspeccion[i].FileName);
                        var max_file_size = 8388608;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };
                       
                        if ( allowed_extensions.Contains(extension) && model.EvidenciasInspeccion[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                           var filePath = await UploadZipFileAsync(model.EvidenciasInspeccion[i], newfileName);
                            evidencias.Add(filePath);
                        }                        
                    }
                }

                await _implementarObservacion.Execute(model.idObservacion, evidencias, model.Comentario, userInfo.NombreUsuario);
                return RedirectToAction("NotificarImplementacionObservacion", "NotificacionesInspecciones", new { IdObservacion = _protector.Protect(model.idObservacion.ToString())});
            }
            var IdEncriptado = _protector.Protect(model.idCabecera.ToString());
            return RedirectToAction("ImplementarInspeccion", new { IdCabecera = IdEncriptado });
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Ev_Imp_Obs_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + nFile.ToString();
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?fileName=" + fileName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre-v2\\";
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
            Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaI;

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
                var SGATempPath = tempPath + "SGA-Marcobre-v2/";
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
                Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaI;

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