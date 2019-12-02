using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarDetalles;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.VerificarObservacion;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.VerificacionMA
{
    public class InspeccionesVerificacionMAController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IConsultarCabecerasVerificacion _consultarCabecerasVerificacion;
        private readonly IConsultarCabeceraVerificacion _consultarCabeceraVerificacion;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IConsultarDetallesVerificacion _consultarDetalleVerificacion;
        private readonly IVerificarObservacionMA _verificarObservacion;

        private readonly IHostingEnvironment _hostingEnvironment;

        public InspeccionesVerificacionMAController(
            ISelectItems selectItems,
            IConsultarCabecerasVerificacion consultarCabecerasVerificacion, 
            IConsultarCabeceraVerificacion consultarCabeceraVerificacion, 
            IConsultarInspeccionDetalle consultarInspeccionDetalle, 
            IConsultarDetallesVerificacion consultarDetalleVerificacion,
            IVerificarObservacionMA verificarObservacion,
            IHostingEnvironment hostingEnvironment)
        {
            _selectItems = selectItems;
            _consultarCabecerasVerificacion = consultarCabecerasVerificacion;
            _consultarCabeceraVerificacion = consultarCabeceraVerificacion;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarDetalleVerificacion = consultarDetalleVerificacion;
            _verificarObservacion = verificarObservacion;
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
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, int filtroGerencia, string filtroCodigo, int pagina, int rpp)
        {
            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarCabecerasVerificacion.Execute(pagina, rpp, fecha, filtroGerencia, filtroCodigo);
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
                    filasTabla += "<td><a href=\"/InspeccionesVerificacionMA/VerificarInspeccion?IdCabecera=" + fila.IdCabecera.ToString() + "\">" + fila.Codigo + "</a></td>";
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
        public async Task<IActionResult> VerificarInspeccion(int IdCabecera)
        {
            if (IdCabecera != 0)
            {
                var cabecera = await _consultarCabeceraVerificacion.Execute(IdCabecera);
                ViewBag.Cabecera = cabecera;
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.ExecuteVerificacion(IdCabecera,User.Identity.Name);
                foreach (var observacion in observaciones)
                {
                    //if (observacion.Estado.Equals(EstadosInspeccion.Implementado.ToString()))
                    //{
                        var evidencia = "<label style=\"width:110px\">";
                        var evidencia_implementacion = "<label style=\"width:110px\">";
                        for (int i = 0; i < observacion.Evidencia_Condicion.Count; i++)
                        {
                            if (i == 0)
                                evidencia = "<a href=\"/InspeccionesVerificacionMA/Download?path=" + observacion.Evidencia_Condicion[i] + "\">Evidencia " + (i + 1) + "</a>";
                            else
                                evidencia += "<a href=\"/InspeccionesVerificacionMA/Download?path=" + observacion.Evidencia_Condicion[i] + "\">, Evidencia " + (i + 1) + "</a>";
                        }
                        evidencia += "</label>";
                        for (int i = 0; i < observacion.Evidencia_Inspeccion.Count; i++)
                        {
                            if (i == 0)
                                evidencia_implementacion = "<a href=\"/InspeccionesImplementacion/Download?path=" + observacion.Evidencia_Inspeccion[i] + "\">Evidencia " + (i + 1) + "</a>";
                            else
                                evidencia_implementacion += "<a href=\"/InspeccionesImplementacion/Download?path=" + observacion.Evidencia_Inspeccion[i] + "\">, Evidencia " + (i + 1) + "</a>";
                        }
                        evidencia_implementacion += "</label>";
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
                        filaObservacion += "<td>" + evidencia_implementacion + "</td>";
                        filaObservacion += "<td>" + observacion.Comentario_Inspeccion + "</td>";
                    if (observacion.Evidencia_Inspeccion.Count!=0 && observacion.Estado!=EstadosInspeccion.AprobadoMA.ToString() && observacion.Estado != EstadosInspeccion.RechazadoMA.ToString() && observacion.Permisos)
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesVerificacionMA/VerificarObservacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Verificar" + "</a></td>";
                    else
                        filaObservacion += "<td><button class=\"btn btn-primary\" href=\"/InspeccionesVerificacionMA/VerificarInspeccion?IdCabecera=" + observacion.Id_Inspeccion_Cabecera.ToString() + "\" disabled>" + "Verificar" + "</button></td>";
                    filaObservacion += "</tr>";
                    }
                //}
                ViewBag.NObservaciones = observaciones.ToList().Count;
                ViewBag.ContenidoTabla = filaObservacion;
            }
            return View();
        }
        public async Task<IActionResult> VerificarObservacion(int IdObservacion)
        {
            ViewBag.IdObservacion = IdObservacion;
            var observacion = await _consultarDetalleVerificacion.Execute(IdObservacion);
            var model = new VerificarObservacionMAModel();
            model.idObservacion = observacion.Id_Inspeccion_Detalle;
            model.idCabecera = observacion.Id_Inspeccion_Cabecera;
            model.TipoInspeccion = observacion.Tipo_Inspeccion;
            model.Asesor_MA_Seguridad = observacion.Asesor_MA_Seguridad;
            model.CondicionIdentificada = observacion.Descripcion_Condicion;
            model.TipoCondicion = observacion.Tipo_Condicion;
            model.Evidencias = observacion.Evidencia_Condicion;
            model.NiveRiesgo = observacion.Nivel_Riesgo_Ambiental;
            model.AccionTomada = observacion.Descripcion_Accion;
            model.ResponsableMarc = observacion.Responsable_Marcobre;
            model.ResponsableCont = observacion.Responsable_Contratista;
            model.FechaProgramada = observacion.Fecha_Programada.ToShortDateString();
            model.EvidenciasInspeccion = observacion.Evidencia_Inspeccion;
            model.ComentarioImplementacion = observacion.Comentario_Inspeccion;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> VerificarObservacion(VerificarObservacionMAModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Accion.Equals("Rechazar"))
                    await _verificarObservacion.Execute(model.idObservacion, model.ComentarioVerificacionMA, EstadosInspeccion.RechazadoMA.ToString(),User.Identity.Name);
                else
                    await _verificarObservacion.Execute(model.idObservacion, model.ComentarioVerificacionMA, EstadosInspeccion.AprobadoMA.ToString(), User.Identity.Name);
            }
            return RedirectToAction("VerificarInspeccion", new { IdCabecera = model.idCabecera });
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