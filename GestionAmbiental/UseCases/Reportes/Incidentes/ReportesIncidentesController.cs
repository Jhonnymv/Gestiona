using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Reportes.Incidentes
{
    public class ReportesIncidentesController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IGetInformacionReportesIncidentes _getInformacionReportesIncidentes;

        public ReportesIncidentesController(ISelectItems selectItems,
            IGetInformacionReportesIncidentes getInformacionReportesIncidentes)
        {
            _selectItems = selectItems;
            _getInformacionReportesIncidentes = getInformacionReportesIncidentes;
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

                var itemsFiltro = await _selectItems.GetSelectItems();

                var tiposIncidente = itemsFiltro.ItemsTipoIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
                tiposIncidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var gerencias = itemsFiltro.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var supervisores = itemsFiltro.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                supervisores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nivelesIncidente = itemsFiltro.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
                nivelesIncidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.FiltroTiposIncidente = tiposIncidente;
                ViewBag.FiltroGerencias = gerencias;
                ViewBag.FiltroSupervisores = supervisores;
                ViewBag.FiltroNiveles = nivelesIncidente;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetRerpotesFechas(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin, string frecuencia)
        {
            var registros = await _getInformacionReportesIncidentes.GetNReportesFecha(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin, frecuencia);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNReportesTipoIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getInformacionReportesIncidentes.GetNReportesTipoIncidente(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNReportesCausasInmediatasIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getInformacionReportesIncidentes.GetNReportesCausaInmediataIncidente(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNReportesCausasBasicasIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getInformacionReportesIncidentes.GetNReportesCausaBasicaIncidente(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNReportesGerenciaIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getInformacionReportesIncidentes.GetNReportesGerenciaIncidente(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNReportesNivelIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getInformacionReportesIncidentes.GetNReportesNivelIncidente(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
    }
}
