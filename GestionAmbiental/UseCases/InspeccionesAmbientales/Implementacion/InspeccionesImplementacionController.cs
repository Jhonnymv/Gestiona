using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.AmpliarFechaObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarDetalles;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ImplementarObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        private readonly IHostingEnvironment _hostingEnvironment;

        private const string CarpetaEvidencias = "ftp://127.0.0.1:2020/InspeccionesAmbientales/EvidenciasImplementacion/";
        public InspeccionesImplementacionController(
            ISelectItems selectItems,
            IConsultarCabecerasImplementacion consultarCabeceras,
            IConsultarCabeceraImplementacion consultarCabecera,
            IConsultarInspeccionDetalle consultarInspeccionDetalle,
            IConsultarDetalleImplementacion consultarDetalleImplementacion,
            IImplementarObservacion implementarObservacion,
            IHostingEnvironment hostingEnvironment)
        {
            _selectItems = selectItems;
            _consultarCabeceras = consultarCabeceras;
            _consultarCabecera = consultarCabecera;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarDetalleImplementacion = consultarDetalleImplementacion;
            _implementarObservacion = implementarObservacion;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Resumen()
        {
            var selectItemsResumen = await _selectItems.ItemVistaResumen();

            var filtroGerencias = selectItemsResumen.GerenciaEjecutora.OrderBy(x=>x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();

            filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Filtro = filtroGerencias;

            return View();
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, int filtroGerencia, string filtroCodigo, int pagina, int rpp)
        {
            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarCabeceras.Execute(pagina, rpp, fecha, filtroGerencia, filtroCodigo, User.Identity.Name);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.nCabeceras % rpp != 0 ? (datosTabla.nCabeceras / rpp) + 1 : datosTabla.nCabeceras / rpp;
            if (datosTabla.nCabeceras > 0)
            {
                foreach (var fila in datosTabla.cabeceras)
                {

                    filasTabla += "<tr>";
                    /*switch (fila.Estado)
                    {
                        case "Pendiente": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/alert-icon.png\"></td>"; break;
                        case "Vencido": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/pending-icon.png\"></td>"; break;
                        default: filasTabla += "<td class=\"alert-column\"></td>"; break;
                    }*/
                    filasTabla += "<td><a href=\"/InspeccionesImplementacion/ImplementarInspeccion?IdCabecera=" + fila.IdCabecera.ToString() + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.GerenEjecMar + "</td>";
                    filasTabla += "<td>" + fila.EmpContratMarc + "</td>";
                    filasTabla += "<td>" + fila.LugarInspec + "</td>";
                    filasTabla += "<td>" + fila.FechaInspec.ToString() + "</td>";
                    filasTabla += "<td>" + fila.NObservaciones + "</td>";
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
        public async Task<IActionResult> ImplementarInspeccion(int IdCabecera)
        {
            if (IdCabecera != 0)
            {
                var cabecera = await _consultarCabecera.Execute(IdCabecera);
                ViewBag.Cabecera = cabecera;
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.Execute(IdCabecera, User.Identity.Name);
                foreach (var observacion in observaciones)
                {
                    var evidencia = "";
                    var evidencia_implementacion = "";
                    for (int i = 0; i < observacion.Evidencia_Condicion.Count; i++)
                        evidencia = "<a href=\"/InspeccionesImplementacion/Download?fileName=" + observacion.Evidencia_Condicion[i] + "\">Evidencia</a>";
                       
                    if (observacion.Evidencia_Inspeccion != null)
                    {
                        for (int i = 0; i < observacion.Evidencia_Inspeccion.Count; i++)
                            evidencia_implementacion = "<a href=\"/InspeccionesImplementacion/Download?fileName=" + observacion.Evidencia_Inspeccion[i] + "\">Evidencia</a>";
                    }
                    filaObservacion += "<tr>";
                    filaObservacion += "<td style=\"display: none; \">" + observacion.Id_Inspeccion_Detalle + "</td>";
                    filaObservacion += "<td>" + observacion.Tipo_Inspeccion + "</td>";
                    filaObservacion += "<td>" + observacion.Asesor_MA_Seguridad + "</td>";
                    filaObservacion += "<td>" + observacion.Descripcion_Condicion + "</td>";
                    filaObservacion += "<td><label style=\"width:250px\">" + observacion.Tipo_Condicion + "</label></td>";
                    filaObservacion += "<td>" + evidencia + "</td>";
                    switch (observacion.Nivel_Riesgo_Ambiental)
                    {
                        case "Tolerable": filaObservacion += "<td><span class=\"badge badge-success\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Alerta": filaObservacion += "<td><span class=\"badge badge-warning\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "No Aceptable": filaObservacion += "<td><span class=\"badge badge-danger\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Bajo": filaObservacion += "<td><span class=\"badge badge-success\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Medio": filaObservacion += "<td><span class=\"badge badge-warning\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Alto": filaObservacion += "<td><span class=\"badge badge-danger\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        default: filaObservacion += "<td><span class=\"badge badge-secondary\">" + observacion.Nivel_Riesgo_Ambiental + "</span>"; break;
                    }
                    filaObservacion += "<td><label style=\"width:200px\">" + observacion.Descripcion_Accion + "</label></td>";
                    filaObservacion += "<td>" + observacion.Responsable_Marcobre + "</td>";
                    filaObservacion += "<td>" + observacion.Responsable_Contratista + "</td>";
                    filaObservacion += "<td>" + observacion.Fecha_Programada.ToShortDateString() + "</td>";
                    if (observacion.Estado.Equals(EstadosInspeccion.Registrado.ToString()) &&
                        observacion.Permisos)
                    {
                        filaObservacion += "<td><span class=\"badge badge-secondary\">Pendiente</span></td>";
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/ImplementarObservacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Implementar" + "</a></td>";
                    }

                    else if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) &&
                        observacion.Permisos)
                    {
                        filaObservacion += "<td><span class=\"badge badge-warning\">Vencido</span></td>";
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/SolicitarAmpliacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Solicitar Ampliación" + "</a></td>";
                    }else if (observacion.Estado.Equals(EstadosInspeccion.Aprobado.ToString()))
                        filaObservacion += "<td><span class=\"badge badge-success\">Aprobado</span></td><td></td>";
                    else if (observacion.Estado.Equals(EstadosInspeccion.Rechazado.ToString()))
                    {
                        filaObservacion += "<td><span class=\"badge badge-danger\">Rechazado</span></td>";
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/ImplementarObservacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Implementar" + "</a></td>";
                    }
                    else if (observacion.Estado.Equals(EstadosInspeccion.Implementado.ToString()))
                    {
                        filaObservacion += "<td><span class=\"badge badge-info\">Implementado</span></td>";
                        filaObservacion += "<td><button class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/ImplementarInspeccion?IdCabecera=" + observacion.Id_Inspeccion_Cabecera.ToString() + "\" disabled>" + "Implementar" + "</button></td>";
                    }
                    else
                    {
                        filaObservacion += "<td><span class=\"badge badge-secondary\">Pendiente</span></td>";
                        filaObservacion += "<td><button class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/ImplementarInspeccion?IdCabecera=" + observacion.Id_Inspeccion_Cabecera.ToString() + "\" disabled>" + "Implementar" + "</button></td>";
                    }
                        filaObservacion += "</tr>";

                }
                ViewBag.ContenidoTabla = filaObservacion;
                ViewBag.NObservaciones = observaciones.Count;
            }
            return View();
        }
        public async Task<IActionResult> ImplementarObservacion(int IdObservacion)
        {
            ViewBag.IdObservacion = IdObservacion;
            var observacion = await _consultarDetalleImplementacion.Execute(IdObservacion);
            var model = new LevantarObservacionModel();
            model.TipoInspeccion = observacion.Tipo_Inspeccion;
            model.Asesor_MA_Seguridad = observacion.Asesor_MA_Seguridad;
            model.idObservacion = observacion.Id_Inspeccion_Detalle;
            model.idCabecera = observacion.Id_Inspeccion_Cabecera;
            model.CondicionIdentificada = observacion.Descripcion_Condicion;
            model.TipoCondicion = observacion.Tipo_Condicion;
            model.Evidencias = observacion.Evidencia_Condicion;
            model.NiveRiesgo = observacion.Nivel_Riesgo_Ambiental;
            model.AccionTomada = observacion.Descripcion_Accion;
            model.ResponsableMarc = observacion.Responsable_Marcobre;
            model.ResponsableCont = observacion.Responsable_Contratista;
            model.FechaProgramada = observacion.Fecha_Programada.ToShortDateString();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ImplementarObservacion(LevantarObservacionModel model)
        {
            if (ModelState.IsValid)
            {
                var evidencias = new List<string>();
                if (model.EvidenciasInspeccion != null)
                {
                    for (int i = 0; i < model.EvidenciasInspeccion.Count; i++)
                    {
                        var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                        var filePath = UploadZipFile(model.EvidenciasInspeccion[i], newfileName);
                        evidencias.Add(filePath);
                    }
                }
                await _implementarObservacion.Execute(model.idObservacion, evidencias, model.Comentario,User.Identity.Name);
                return RedirectToAction("NotificarImplementacionObservacion", "Notificaciones", new { IdObservacion = model.idObservacion });
            }
            return RedirectToAction("ImplementarInspeccion", new { IdCabecera = model.idCabecera });
        }
        public async Task<IActionResult> SolicitarAmpliacion(int IdObservacion)
        {
            //notificar

            var response = "";
            return Ok(response);
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia_Inspecciones_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + nFile.ToString();
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
                {".ppt", "application/vnd.ms-powerpoint" },
                {".pptx", "application/vnd.ms-powerpoint" }
            };
        }

        public IActionResult Download(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(CarpetaEvidencias + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential("SGA", "Pa$$W0rd");
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


                return File(ftpStream, GetContentType(fileName), Path.GetFileName(fileName));
            }
            catch (Exception)
            {
                return Content("No se encontró el archivo");
            }
        }
        public string UploadFTPFile(string path, string newFileName)
        {
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(CarpetaEvidencias + newFileName);

            ftpRequest.Credentials = new NetworkCredential("SGA", "Pa$$W0rd");
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
                requestStream.WriteAsync(buffer, 0, buffer.Length);
                requestStream.FlushAsync();
                requestStream.Close();
            }
            return newFileName;
        }
        public string UploadZipFile(IFormFile file, string newFileName)
        {
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "SGA-Marcobre/";
            if (!Directory.Exists(SGATempPath))
            {
                Directory.CreateDirectory(SGATempPath);
            }
            var filePath = SGATempPath + "submission/" + User.Identity.Name.Split("\\")[1] + "/";
            var archiveFile = SGATempPath + "zip/" + User.Identity.Name.Split("\\")[1] + "/archive.zip";
            var archivePath = SGATempPath + "zip/" + User.Identity.Name.Split("\\")[1] + "/";

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
            return UploadFTPFile(archiveFile, newFileName);
        }
    }
}