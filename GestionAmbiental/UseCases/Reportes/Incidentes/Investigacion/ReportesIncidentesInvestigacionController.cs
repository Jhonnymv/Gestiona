using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetCantidadProductoIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoAgua;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoFauna;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoFlora;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoPersona;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoSuelo;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetInvestigaciones;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetNInvestigacionesCausasBasicasIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetNInvestigacionesCausasInmediatasIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetNInvestigacionesGerenciaIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetNInvestigacionesNivelIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetNInvestigacionesTipoIncidente;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetResiduoImpactoAgua;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetResiduoImpactoFauna;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetResiduoImpactoFlora;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetResiduoImpactoSuelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.Incidentes.Investigacion
{
    public class ReportesIncidentesInvestigacionController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IGetInvestigaciones _getInvestigaciones;
        private readonly IGetCantidadProductoIncidente _getCantidadProductoIncidente;
        private readonly IGetImpactoAgua _getImpactoAgua;
        private readonly IGetImpactoFauna _getImpactoFauna;
        private readonly IGetImpactoFlora _getImpactoFlora;
        private readonly IGetImpactoPersona _getImpactoPersona;
        private readonly IGetImpactoSuelo _getImpactoSuelo;
        private readonly IGetNInvestigacionesCausasBasicasIncidente _getNInvestigacionesCausasBasicasIncidente;
        private readonly IGetNInvestigacionesCausasInmediatasIncidente _getNInvestigacionesCausasInmediatasIncidente;
        private readonly IGetNInvestigacionesGerenciaIncidente _getNInvestigacionesGerenciaIncidente;
        private readonly IGetNInvestigacionesNivelIncidente _getNInvestigacionesNivelIncidente;
        private readonly IGetNInvestigacionesTipoIncidente _getNInvestigacionesTipoIncidente;
        private readonly IGetResiduoImpactoAgua _getResiduoImpactoAgua;
        private readonly IGetResiduoImpactoFauna _getResiduoImpactoFauna;
        private readonly IGetResiduoImpactoFlora _getResiduoImpactoFlora;
        private readonly IGetResiduoImpactoSuelo _getResiduoImpactoSuelo;

        public ReportesIncidentesInvestigacionController(
            ISelectItems selectItems,
            IGetInvestigaciones getInvestigaciones,
            IGetCantidadProductoIncidente getCantidadProductoIncidente,
            IGetImpactoAgua getImpactoAgua, 
            IGetImpactoFauna getImpactoFauna,
            IGetImpactoFlora getImpactoFlora,
            IGetImpactoPersona getImpactoPersona,
            IGetImpactoSuelo getImpactoSuelo,
            IGetNInvestigacionesCausasBasicasIncidente getNInvestigacionesCausasBasicasIncidente,
            IGetNInvestigacionesCausasInmediatasIncidente getNInvestigacionesCausasInmediatasIncidente,
            IGetNInvestigacionesGerenciaIncidente getNInvestigacionesGerenciaIncidente, 
            IGetNInvestigacionesNivelIncidente getNInvestigacionesNivelIncidente,
            IGetNInvestigacionesTipoIncidente getNInvestigacionesTipoIncidente,
            IGetResiduoImpactoAgua getResiduoImpactoAgua,
            IGetResiduoImpactoFauna getResiduoImpactoFauna,
            IGetResiduoImpactoFlora getResiduoImpactoFlora,
            IGetResiduoImpactoSuelo getResiduoImpactoSuelo)
        {
            _selectItems = selectItems;
            _getInvestigaciones = getInvestigaciones;
            _getCantidadProductoIncidente = getCantidadProductoIncidente;
            _getImpactoAgua = getImpactoAgua;
            _getImpactoFauna = getImpactoFauna;
            _getImpactoFlora = getImpactoFlora;
            _getImpactoPersona = getImpactoPersona;
            _getImpactoSuelo = getImpactoSuelo;
            _getNInvestigacionesCausasBasicasIncidente = getNInvestigacionesCausasBasicasIncidente;
            _getNInvestigacionesCausasInmediatasIncidente = getNInvestigacionesCausasInmediatasIncidente;
            _getNInvestigacionesGerenciaIncidente = getNInvestigacionesGerenciaIncidente;
            _getNInvestigacionesNivelIncidente = getNInvestigacionesNivelIncidente;
            _getNInvestigacionesTipoIncidente = getNInvestigacionesTipoIncidente;
            _getResiduoImpactoAgua = getResiduoImpactoAgua;
            _getResiduoImpactoFauna = getResiduoImpactoFauna;
            _getResiduoImpactoFlora = getResiduoImpactoFlora;
            _getResiduoImpactoSuelo = getResiduoImpactoSuelo;
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
        public async Task<IActionResult> GetInvestigacionesFechas(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin, string frecuencia)
        {
            var registros = await _getInvestigaciones.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin, frecuencia);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetNInvestigacionesTipoIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNInvestigacionesTipoIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetCantidadProductoIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getCantidadProductoIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetImpactoAgua(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getImpactoAgua.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetImpactoFauna(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getImpactoFauna.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetImpactoFlora(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getImpactoFlora.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetImpactoPersona(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getImpactoPersona.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetImpactoSuelo(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getImpactoSuelo.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNInvestigacionesCausasBasicasIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNInvestigacionesCausasBasicasIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNInvestigacionesCausasInmediatasIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNInvestigacionesCausasInmediatasIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNInvestigacionesGerenciaIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNInvestigacionesGerenciaIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetNInvestigacionesNivelIncidente(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getNInvestigacionesNivelIncidente.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetResiduoImpactoAgua(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getResiduoImpactoAgua.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetResiduoImpactoFauna(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getResiduoImpactoFauna.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetResiduoImpactoFlora(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getResiduoImpactoFlora.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> GetResiduoImpactoSuelo(int tipo_incidente, int nivel_incidente, int gerencia, int supervisor, string fechaInicio, string fechaFin)
        {
            var registros = await _getResiduoImpactoSuelo.Execute(tipo_incidente, nivel_incidente, gerencia, supervisor, fechaInicio, fechaFin);

            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
    }
}