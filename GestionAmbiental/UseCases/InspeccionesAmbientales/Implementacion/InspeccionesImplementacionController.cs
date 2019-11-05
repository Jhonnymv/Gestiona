using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarDetalles;
using GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ImplementarObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
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
            var datosTabla = await _consultarCabeceras.Execute(1, 1, fecha, filtroGerencia, filtroCodigo);
            var filasTabla = "";
            if (datosTabla.Count > 0)
            {
                foreach (var fila in datosTabla)
                {

                    filasTabla += "<tr>";
                    switch (fila.Estado)
                    {
                        case "Pendiente": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/alert-icon.png\"></td>"; break;
                        case "Vencido": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/pending-icon.png\"></td>"; break;
                        default: filasTabla += "<td class=\"alert-column\"></td>"; break;
                    }
                    //filasTabla += "<td><a onclick=\"abrir("+fila.IdCabecera.ToString()+")\" href=\"#\">" + fila.Codigo + "</a></td>";                    
                    filasTabla += "<td><a href=\"/InspeccionesImplementacion/ImplementarInspeccion?IdCabecera=" + fila.IdCabecera.ToString()+"\">" + fila.Codigo + "</a></td>";
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
        public async Task<IActionResult> ImplementarInspeccion(int IdCabecera)
        {
            if (IdCabecera != 0)
            {
                var cabecera = await _consultarCabecera.Execute(IdCabecera);
                ViewBag.Cabecera = cabecera;
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.Execute(IdCabecera);
                foreach (var observacion in observaciones)
                {
                    if (!observacion.Fecha_Cierre.HasValue)
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
                            case "Tolerable": filaObservacion += "<td style:\"background-color:green\">" + observacion.Nivel_Riesgo_Ambiental + "</td>"; break;
                            case "Alerta": filaObservacion += "<td style:\"background-color:yellow\">" + observacion.Nivel_Riesgo_Ambiental + "</td>"; break;
                            case "No Aceptable": filaObservacion += "<td style:\"background-color:red\">" + observacion.Nivel_Riesgo_Ambiental + "</td>"; break;
                            default: filaObservacion += "<td>" + observacion.Nivel_Riesgo_Ambiental + "</td>"; break;
                        }
                        filaObservacion += "<td>" + observacion.Descripcion_Accion + "</td>";
                        filaObservacion += "<td>" + observacion.Responsable_Marcobre + "</td>";
                        filaObservacion += "<td>" + observacion.Responsable_Contratista + "</td>";
                        filaObservacion += "<td>" + observacion.Fecha_Programada.ToShortDateString() + "</td>";
                        filaObservacion += "<td><a href=\"/InspeccionesImplementacion/ImplementarObservacion?IdObservacion=" + observacion.Id_Inspeccion_Detalle.ToString() + "\">" + "Inspeccionar" + "</a></td>";
                        filaObservacion += "</tr>";
                    }
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
            model.idObservacion = observacion.Id_Inspeccion_Detalle;
            model.idCabecera = observacion.Id_Inspeccion_Cabecera;
            model.CondicionIdentificada = observacion.Descripcion_Condicion;
            model.TipoCondicion = observacion.Tipo_Condicion.Descripcion;
            model.Evidencias = observacion.Evidencia_Condicion;
            model.NiveRiesgo = observacion.Nivel_Riesgo_Ambiental;
            model.AccionTomada = observacion.Descripcion_Accion;
            model.ResponsableMarc = observacion.Responsable_Marcobre.Nombres+" "+observacion.Responsable_Marcobre.Apellidos;
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
                        var filepath = Path.Combine(_hostingEnvironment.WebRootPath, fileName);

                        await model.EvidenciasInspeccion[i].CopyToAsync(new FileStream(filepath, FileMode.Create));
                        evidencias.Add(filepath);
                    }
                }
                await _implementarObservacion.Execute(model.idObservacion, evidencias, model.Comentario);
            }
            return RedirectToAction("ImplementarInspeccion", new { IdCabecera = model.idCabecera });
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia_Inspecciones_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + nFile.ToString();
            return fileName;
        }
    }
}