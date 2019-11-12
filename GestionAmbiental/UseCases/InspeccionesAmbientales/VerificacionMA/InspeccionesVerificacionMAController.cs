using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarDetalles;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.VerificarObservacion;
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

        public InspeccionesVerificacionMAController(
            ISelectItems selectItems,
            IConsultarCabecerasVerificacion consultarCabecerasVerificacion, 
            IConsultarCabeceraVerificacion consultarCabeceraVerificacion, 
            IConsultarInspeccionDetalle consultarInspeccionDetalle, 
            IConsultarDetallesVerificacion consultarDetalleVerificacion,
            IVerificarObservacionMA verificarObservacion)
        {
            _selectItems = selectItems;
            _consultarCabecerasVerificacion = consultarCabecerasVerificacion;
            _consultarCabeceraVerificacion = consultarCabeceraVerificacion;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarDetalleVerificacion = consultarDetalleVerificacion;
            _verificarObservacion = verificarObservacion;
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
            var datosTabla = await _consultarCabecerasVerificacion.Execute(1, 1, fecha, filtroGerencia, filtroCodigo);
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
                    filasTabla += "<td><a href=\"/InspeccionesVerificacionMA/VerificarInspeccion?IdCabecera=" + fila.IdCabecera.ToString() + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.GerenEjecMar + "</td>";
                    filasTabla += "<td>" + fila.EmpContratMarc + "</td>";
                    filasTabla += "<td>" + fila.GerenEjecEPCM + "</td>";
                    filasTabla += "<td>" + fila.EmpContratEPCM + "</td>";
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
        public async Task<IActionResult> VerificarInspeccion(int IdCabecera)
        {
            if (IdCabecera != 0)
            {
                var cabecera = await _consultarCabeceraVerificacion.Execute(IdCabecera);
                ViewBag.Cabecera = cabecera;
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.Execute(IdCabecera);
                foreach (var observacion in observaciones)
                {
                    if (observacion.Estado.Equals("Aprobado"))
                    {
                        var evidencia = "";
                        for (int i = 0; i < observacion.Evidencia_Condicion.Count; i++)
                        {
                            if (i == 0)
                                evidencia = "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">Evidencia " + (i + 1) + "</a>";
                            else
                                evidencia += "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">, Evidencia " + (i + 1) + "</a>";
                        }
                        filaObservacion += "<tr>";
                        filaObservacion += "<td style=\"display: none; \">" + observacion.Id_Inspeccion_Detalle + "</td>";
                        filaObservacion += "<td>" + observacion.Descripcion_Condicion + "</td>";
                        filaObservacion += "<td>" + observacion.Tipo_Condicion + "</td>";
                        filaObservacion += "<td>" + evidencia + "</td>";
                        switch (observacion.Nivel_Riesgo_Ambiental)
                        {
                            case "Tolerable": filaObservacion += "<td><span class=\"badge badge-success\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                            case "Alerta": filaObservacion += "<td><span class=\"badge badge-warning\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                            case "No Aceptable": filaObservacion += "<td><span class=\"badge badge-danger\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                            default: filaObservacion += "<td><span class=\"badge badge-secondary\">" + observacion.Nivel_Riesgo_Ambiental + "</span>"; break;
                        }
                        filaObservacion += "<td>" + observacion.Descripcion_Accion + "</td>";
                        filaObservacion += "<td>" + observacion.Responsable_Marcobre + "</td>";
                        filaObservacion += "<td>" + observacion.Responsable_Contratista + "</td>";
                        filaObservacion += "<td>" + observacion.Fecha_Programada.ToShortDateString() + "</td>";
                        filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesVerificacionMA/VerificarObservacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Verificar" + "</a></td>";
                        filaObservacion += "</tr>";
                    }
                }
                ViewBag.NObservaciones = observaciones.Count;
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
            model.CondicionIdentificada = observacion.Descripcion_Condicion;
            model.TipoCondicion = observacion.Tipo_Condicion.Descripcion;
            model.Evidencias = observacion.Evidencia_Condicion;
            model.NiveRiesgo = observacion.Nivel_Riesgo_Ambiental;
            model.AccionTomada = observacion.Descripcion_Accion;
            model.ResponsableMarc = observacion.Responsable_Marcobre.Nombres + " " + observacion.Responsable_Marcobre.Apellidos;
            model.ResponsableCont = observacion.Responsable_Contratista;
            model.FechaProgramada = observacion.Fecha_Programada.ToShortDateString();
            model.EvidenciasInspeccion = observacion.Evidencia_Inspeccion;
            model.ComentarioImplementacion = observacion.Comentario_Inspeccion;
            model.ComentarioVerificacion = observacion.Comentario_Inspector;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> VerificarObservacion(VerificarObservacionMAModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Accion.Equals("Rechazar"))
                    await _verificarObservacion.Execute(model.idObservacion, model.ComentarioVerificacion, "RechazadoMA");
                else
                    await _verificarObservacion.Execute(model.idObservacion, model.ComentarioVerificacion, "AprobadoMA");
            }
            return RedirectToAction("VerificarInspeccion", new { IdCabecera = model.idCabecera });
        }
    }
}