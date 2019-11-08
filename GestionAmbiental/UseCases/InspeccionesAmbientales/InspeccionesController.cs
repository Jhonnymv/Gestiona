using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.EditarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales
{
    public class InspeccionesController : Controller
    {
        private readonly IConsultarCabeceras _consultarCabeceras;
        private readonly IConsultarCabecera _consultarCabecera;
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarInspeccionCabecera _registrarInspeccionCabecera;
        private readonly IRegistrarInspeccionObservacion _registrarInspeccionObservacion;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IEditarInspeccionCabecera _editarInspeccionCabecera;
        public InspeccionesController(
            IConsultarCabeceras consultarCabeceras,
            ISelectItems selectItems,
            IRegistrarInspeccionCabecera registrarInspeccionCabecera,
            IHostingEnvironment hostingEnvironment,
            IRegistrarInspeccionObservacion registrarInspeccionObservacion,
            IConsultarInspeccionDetalle consultarInspeccionDetalle,
            IConsultarCabecera consultarCabecera,
            IEditarInspeccionCabecera editarInspeccionCabecera)
        {
            _consultarCabeceras = consultarCabeceras;
            _selectItems = selectItems;
            _registrarInspeccionCabecera = registrarInspeccionCabecera;
            _hostingEnvironment = hostingEnvironment;
            _registrarInspeccionObservacion = registrarInspeccionObservacion;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarCabecera = consultarCabecera;
            _editarInspeccionCabecera = editarInspeccionCabecera;
        }
        public async Task<IActionResult> Resumen()
        {
            var selectItemsResumen = await _selectItems.ItemVistaResumen();

            var filtroGerencias = selectItemsResumen.GerenciaEjecutora.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            filtroGerencias.Insert(0, new SelectListItem("No Aplica", "1"));
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
                    //href="/Inspecciones/NuevaInspeccion"
                    filasTabla += "<td><a href=\"/Inspecciones/EditarInspeccion?IdCabecera="+fila.IdCabecera+"\">" + fila.Codigo + "</a></td>";
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
        public async Task<IActionResult> NuevaInspeccion()
        {
            //Cargar observaciones si se tiene
            
            var selectItemsResumen = await _selectItems.ItemVistaNuevaInspeccion();

            var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var gerenciasEPCM = selectItemsResumen.GerenciasEPCM.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEPCM.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var empresasMC = selectItemsResumen.EmpContratMarcobre.Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasMC.Insert(0, new SelectListItem("No Aplica", "1"));
            empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var empresasEPCM = selectItemsResumen.EmpContratEPCM.Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasEPCM.Insert(0, new SelectListItem("No Aplica", "1"));
            empresasEPCM.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var responsables = selectItemsResumen.Usuarios.Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " "+ g.Apellidos }).ToList();
            responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var idCabecera = Convert.ToInt32(TempData["idCabecera"]);
            if (idCabecera != 0)
            {
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.Execute(idCabecera);
                ViewBag.nObservacioness = observaciones.Count;
               
                foreach (var observacion in observaciones)
                {
                    var evidencia = "";
                    for (int i = 0; i < observacion.Evidencia_Condicion.Count; i++)
                    {
                        if (i == 0)
                            evidencia = "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">Evidencia " + (i+1) + "</a>";
                        else
                            evidencia += "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">, Evidencia " + (i+1) + "</a>";
                    }
                    filaObservacion += "<tr>";
                    filaObservacion += "<td style=\"display: none; \">" + observacion.Id_Inspeccion_Detalle + "</td>";
                    filaObservacion += "<td>" + observacion.Descripcion_Condicion + "</td>";
                    filaObservacion += "<td>" + observacion.Tipo_Condicion + "</td>";
                    filaObservacion += "<td>" + evidencia + "</td>";
                    filaObservacion += "<td>" + observacion.Nivel_Riesgo_Ambiental + "</td>";
                    filaObservacion += "<td>" + observacion.Descripcion_Accion + "</td>";
                    filaObservacion += "<td>" + observacion.Responsable_Marcobre + "</td>";
                    filaObservacion += "<td>" + observacion.Responsable_Contratista + "</td>";
                    filaObservacion += "<td>" + observacion.Fecha_Programada + "</td>";
                    filaObservacion += "</tr>";

                }
                ViewBag.ContenidoTabla = filaObservacion;
            }

            ViewBag.GerenciaMC = gerenciasEjecutorasMC;
            ViewBag.GerenciaEPCM = gerenciasEPCM;
            ViewBag.EmpresasMC = empresasMC;
            ViewBag.EmpresasEPCM = empresasEPCM;
            ViewBag.Responsables = responsables;

            return View(new NuevaInspeccionModel());
        }
        [HttpPost]
        public async Task<IActionResult> NuevaInspeccion(NuevaInspeccionModel model)
        {
            
            if (ModelState.IsValid)
            {
                int idCabecera = await _registrarInspeccionCabecera.Execute(
                model.Objetivo, model.Id_Gerencia_Ejecutora, model.Id_Empresa_Contratista, model.Id_Gerencia_EPCM,
                model.Id_Empresa_Contratista_EPCM, model.Lugar_Inspeccionado, model.Fecha_Inspeccion, model.Hora_Inspeccion,
                model.Id_Responsable_Lugar, model.Id_Responsable_MA, model.Inspectores);
                TempData["idCabecera"] = idCabecera;
                if(model.opcion.Equals("Observacion"))
                    return RedirectToAction("NuevaObservacion", new { IdCabecera =  idCabecera});
                else
                    return RedirectToAction("NuevaInspeccion");
            }
            else
                return RedirectToAction("Resumen");
        }
        
        public async Task<IActionResult> NuevaObservacion(int IdCabecera)
        {

            var selectItemsResumen = await _selectItems.ItemVistaNuevaObservacion();

            var tiposCondicion = selectItemsResumen.TipoCondicion.Select(g => new SelectListItem() { Value = g.Id_Tipo_Condicion.ToString(), Text = g.Descripcion }).ToList();
            tiposCondicion.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var responsables = selectItemsResumen.Usuarios.Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

           

            ViewBag.TiposCondicion = tiposCondicion;
            ViewBag.Responsables = responsables;
            ViewBag.idCabecera = IdCabecera;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevaObservacion(NuevaObservacionModel model)
        {
            if (ModelState.IsValid)
            {
                var evidencias = new List<string>();
                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {
                        //TODO agregar extension al archivo
                        var fileExtension = Path.GetExtension(model.Evidencias[i].FileName);
                        var fileName = CreateUniqueFileName(i + 1) + fileExtension;
                        var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "InspeccionesAmbientales", "EvidenciasRegistro", fileName);
                        var file = new FileStream(filepath, FileMode.Create);
                        await model.Evidencias[i].CopyToAsync(file);
                        file.Close();
                        evidencias.Add(filepath);
                    }
                }
                var idObservacion = await _registrarInspeccionObservacion.Execute(
                            model.idCabecera,
                            model.CondicionIdentificada,
                            model.TipoCondicion,
                            evidencias,
                            model.NiveRiesgo,
                            model.AccionTomada,
                            model.ResponsableMarc,
                            model.ResponsableCont,
                            model.FechaProgramada);

                return RedirectToAction("NotificarNuevaObservacionAsignada", "Notificaciones", new { IdObservacion = idObservacion });
            }
            else
                return RedirectToAction("NuevaObservacion", model);
            
        }
        public async Task<IActionResult> EditarInspeccion(int IdCabecera)
        {
            //var idCabecera = Convert.ToInt32(TempData["idCabecera"]);
            var selectItemsResumen = await _selectItems.ItemVistaEditarInspeccion();

            var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var gerenciasEPCM = selectItemsResumen.GerenciasEPCM.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEPCM.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var empresasMC = selectItemsResumen.EmpContratMarcobre.Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasMC.Insert(0, new SelectListItem("No Aplica", "1"));
            empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var empresasEPCM = selectItemsResumen.EmpContratEPCM.Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasEPCM.Insert(0, new SelectListItem("No Aplica", "1"));
            empresasEPCM.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var responsables = selectItemsResumen.Usuarios.Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            if (IdCabecera != 0)
            {
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.Execute(IdCabecera);
                ViewBag.nObservacioness = observaciones.Count;

                foreach (var observacion in observaciones)
                {
                    var evidencia = "";
                    for (int i = 0; i < observacion.Evidencia_Condicion.Count; i++)
                    {
                        /* if (i == 0)
                             evidencia = "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">Evidencia " + (i+1) + "</a>";
                         else
                             evidencia += "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">, Evidencia " + (i+1) + "</a>";*/
                        if (i == 0)
                            evidencia = evidencia+"<a href=\"/Inspecciones/Download?path="+ observacion.Evidencia_Condicion[i] + "\">Evidencia " + (i + 1) + "</a>";
                        else
                            evidencia = evidencia+"<a href=\"/Inspecciones/Download?path=" + observacion.Evidencia_Condicion[i] + "\">, Evidencia " + (i + 1) + "</a>";
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
                    filaObservacion += "</tr>";

                }
                ViewBag.ContenidoTabla = filaObservacion;
            }


            ViewBag.GerenciaMC = gerenciasEjecutorasMC;
            ViewBag.GerenciaEPCM = gerenciasEPCM;
            ViewBag.EmpresasMC = empresasMC;
            ViewBag.EmpresasEPCM = empresasEPCM;
            ViewBag.Responsables = responsables;

            var cabecera = await _consultarCabecera.Execute(IdCabecera);
            var model = new NuevaInspeccionModel();

            if (cabecera != null)
            {
                model.Id_Inspeccion_Cabecera = IdCabecera;
                model.Codigo = cabecera.Codigo;
                model.Id_Empresa_Contratista = cabecera.idEmpContratMarc;
                model.Id_Empresa_Contratista_EPCM = cabecera.idEmpContratEPCM;
                model.Id_Gerencia_Ejecutora = cabecera.idGerenciaEjecMar;
                model.Id_Gerencia_EPCM = cabecera.idGerencEjectEPCM;
                model.Lugar_Inspeccionado = cabecera.LugarInspec;
                model.Id_Responsable_Lugar = cabecera.idResponsableLugar;
                model.Id_Responsable_MA = cabecera.idResponsableMA;
                model.Fecha_Inspeccion = cabecera.FechaInspec.Year + "-" + (cabecera.FechaInspec.Month.ToString().Length>1? cabecera.FechaInspec.Month.ToString():"0"+ cabecera.FechaInspec.Month.ToString()) + "-" + (cabecera.FechaInspec.Day.ToString().Length > 1 ? cabecera.FechaInspec.Day.ToString() : "0" + cabecera.FechaInspec.Day.ToString());
                model.Hora_Inspeccion = (cabecera.FechaInspec.Hour.ToString().Length > 1 ? cabecera.FechaInspec.Hour.ToString() : "0" + cabecera.FechaInspec.Hour.ToString()) + ":" + (cabecera.FechaInspec.Minute.ToString().Length > 1 ? cabecera.FechaInspec.Minute.ToString() : "0" + cabecera.FechaInspec.Minute.ToString());
                model.Objetivo = cabecera.Objetivo;
                model.Inspectores = cabecera.Inspectores;
            }
            //TempData["idCabecera"] = IdCabecera;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditarInspeccion(NuevaInspeccionModel model)
        {
            if (ModelState.IsValid)
            {
                await _editarInspeccionCabecera.Execute(
                    model.Id_Inspeccion_Cabecera,
                    model.Objetivo,
                    model.Id_Gerencia_Ejecutora,
                    model.Id_Empresa_Contratista,
                    model.Id_Gerencia_EPCM,
                    model.Id_Empresa_Contratista_EPCM,
                    model.Lugar_Inspeccionado,
                    model.Fecha_Inspeccion,
                    model.Hora_Inspeccion,
                    model.Id_Responsable_Lugar,
                    model.Id_Responsable_MA,
                    model.Inspectores
                    );
                if (model.opcion.Equals("Observacion"))
                    return RedirectToAction("NuevaObservacion", new { IdCabecera = model.Id_Inspeccion_Cabecera });
                else
                    return RedirectToAction("Resumen");
            }
            return RedirectToAction("Resumen");
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
                {".csv", "text/csv"}
            };
        }
    }
}