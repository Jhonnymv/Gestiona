using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.ReportePreliminar.GetNReportesCausaIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.ReportePreliminar.GetNReportesGerencia;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.ReportePreliminar.GetNReportesNivelIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.ReportePreliminar.GetNReportesTipoIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.ReportePreliminar.GetReportesPreliminares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.Incidentes.ReportePreliminar
{
    public class ReportesIncidentesReportePreliminarController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IGetReportesPreliminares _getReportesPreliminares;
        private readonly IGetNReportesTipoIncidente _getNReportesTipoIncidente;
        private readonly IGetNReportesNivelIncidente _getNReportesNivelIncidente;
        private readonly IGetNReportesCausaIncidente _getNReportesCausaIncidente;
        private readonly IGetNReportesGerencia _getNReportesGerencia;

        public ReportesIncidentesReportePreliminarController(
            ISelectItems selectItems,
            IGetReportesPreliminares getReportesPreliminares,
            IGetNReportesTipoIncidente getNReportesTipoIncidente,
            IGetNReportesNivelIncidente getNReportesNivelIncidente,
            IGetNReportesCausaIncidente getNReportesCausaIncidente,
            IGetNReportesGerencia getNReportesGerencia)
        {
            _selectItems = selectItems;
            _getReportesPreliminares = getReportesPreliminares;
            _getNReportesTipoIncidente = getNReportesTipoIncidente;
            _getNReportesNivelIncidente = getNReportesNivelIncidente;
            _getNReportesCausaIncidente = getNReportesCausaIncidente;
            _getNReportesGerencia = getNReportesGerencia;
        }

        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
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
        public async Task<IActionResult> GetRerpotesPreliminaresFechas(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin,string frecuencia)
        {
            var registros = await _getReportesPreliminares.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin,frecuencia);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRerpotesPreliminaresTipoIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNReportesTipoIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRerpotesPreliminaresNivelIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNReportesNivelIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRerpotesPreliminaresGerencia(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNReportesGerencia.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetRerpotesPreliminaresCausaIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNReportesCausaIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
    }
}