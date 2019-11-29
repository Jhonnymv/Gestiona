using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.AmpliarFechaObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarDetalles;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ImplementarObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
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

            var filtroGerencias = selectItemsResumen.GerenciaEjecutora.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();

            filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Filtro = filtroGerencias;

            return View();
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, int filtroGerencia, string filtroCodigo)
        {
            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarCabeceras.Execute(1, 1, fecha, filtroGerencia, filtroCodigo, User.Identity.Name);
            var filasTabla = "";
            if (datosTabla.Count > 0)
            {
                foreach (var fila in datosTabla)
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
                    {
                        if (i == 0)
                            evidencia = "<a href=\"/InspeccionesImplementacion/Download?path=" + observacion.Evidencia_Condicion[i] + "\">Evidencia " + (i + 1) + "</a>";
                        else
                            evidencia += "<a href=\"/InspeccionesImplementacion/Download?path=" + observacion.Evidencia_Condicion[i] + "\">, Evidencia " + (i + 1) + "</a>";
                    }
                    if (observacion.Evidencia_Inspeccion != null)
                    {
                        for (int i = 0; i < observacion.Evidencia_Inspeccion.Count; i++)
                        {
                            if (i == 0)
                                evidencia_implementacion = "<a href=\"/InspeccionesImplementacion/Download?path=" + observacion.Evidencia_Inspeccion[i] + "\">Evidencia " + (i + 1) + "</a>";
                            else
                                evidencia_implementacion += "<a href=\"/InspeccionesImplementacion/Download?path=" + observacion.Evidencia_Inspeccion[i] + "\">, Evidencia " + (i + 1) + "</a>";
                        }
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
                        observacion.PermisosImplementar)
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/ImplementarObservacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Implementar" + "</a></td>";
                    else if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) &&
                        observacion.PermisosImplementar)
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/SolicitarAmpliacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Solicitar Ampliación" + "</a></td>";
                    else if (observacion.Estado.Equals(EstadosInspeccion.AprobadoMA.ToString()))
                        filaObservacion += "<td><span class=\"badge badge-success\">Aprobado</span></td>";
                    else if (observacion.Estado.Equals(EstadosInspeccion.RechazadoMA.ToString()))
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/ImplementarObservacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Implementar Rechazado" + "</a></td>";
                    else
                        filaObservacion += "<td><button class=\"btn btn-primary\" href=\"/InspeccionesImplementacion/ImplementarInspeccion?IdCabecera=" + observacion.Id_Inspeccion_Cabecera.ToString() + "\" disabled>" + "Implementar" + "</button></td>";
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
                        //TODO agregar extension al archivo
                        var fileExtension = Path.GetExtension(model.EvidenciasInspeccion[i].FileName);
                        var fileName = CreateUniqueFileName(i + 1) + fileExtension;
                        var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "InspeccionesAmbientales", "EvidenciasImplementacion", fileName);
                        var file = new FileStream(filepath, FileMode.Create);
                        await model.EvidenciasInspeccion[i].CopyToAsync(file);
                        file.Close();
                        evidencias.Add(filepath);
                    }
                }
                await _implementarObservacion.Execute(model.idObservacion, evidencias, model.Comentario);
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
        public async Task<IActionResult> Download(string path)
        {
            if (path == null)
                return Content("No se encontró el archivo");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
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
    }
}