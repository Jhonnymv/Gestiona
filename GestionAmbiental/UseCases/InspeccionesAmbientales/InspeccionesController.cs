using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarCabeceras;
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
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarInspeccion _registrarInspeccion;
        private readonly IHostingEnvironment _hostingEnvironment;
        public InspeccionesController(IConsultarCabeceras consultarCabeceras, ISelectItems selectItems, IRegistrarInspeccion registrarInspeccion, IHostingEnvironment hostingEnvironment )
        {
            _consultarCabeceras = consultarCabeceras;
            _selectItems = selectItems;
            _registrarInspeccion = registrarInspeccion;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Resumen()
        {
            var selectItemsResumen = await _selectItems.Execute();

            var filtroGerencias = selectItemsResumen.GerenciaEjecutora.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();

            filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var gerenciasEPCM = selectItemsResumen.GerenciasEPCM.Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEPCM.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var empresasMC = selectItemsResumen.EmpContratMarcobre.Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var empresasEPCM = selectItemsResumen.EmpContratEPCM.Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasEPCM.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposCondicion = selectItemsResumen.TipoCondicion.Select(g => new SelectListItem() { Value = g.Id_Tipo_Condicion.ToString(), Text = g.Descripcion }).ToList();
            tiposCondicion.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Filtro = filtroGerencias;
            ViewBag.GerenciaMC = gerenciasEjecutorasMC;
            ViewBag.GerenciaEPCM = gerenciasEPCM;
            ViewBag.EmpresasMC = empresasMC;
            ViewBag.EmpresasEPCM = empresasEPCM;
            ViewBag.TiposCondicion = tiposCondicion;

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
                    filasTabla += "<td><a href=\"#\">" + fila.Codigo + "</a></td>";
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
        [HttpPost]
        public async Task<IActionResult> NuevaInspeccion(NuevaInspeccionModel model)
        {
            if (ModelState.IsValid)
            {
                //Guardar archivos en el servidor y obtener el path generado para guardarlo en la bd
                if (model.Observacion.Evidencias!= null)
                {
                    for (int i = 0; i < model.Observacion.Evidencias.Count; i++)
                    {
                        //TODO agregar extension al archivo
                        var fileName = CreateUniqueFileName(i);
                        var filepath = Path.Combine(_hostingEnvironment.WebRootPath, fileName);
                        await model.Observacion.Evidencias[i].CopyToAsync(new FileStream(filepath, FileMode.Create));
                    }
                }
            }

            var response = "";
            return Ok(response);
        }

        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia_Inspecciones_" + User.Identity.Name + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + nFile.ToString();
            return fileName;
        }
        
    }
}