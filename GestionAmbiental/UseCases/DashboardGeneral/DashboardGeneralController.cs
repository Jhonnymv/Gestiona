using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.InspeccionesActoCondicionBP;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.InspeccionesEstadosCondicion;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaReportante;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingUsuariosIngresoInspecciones;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingUsuariosObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopActosSubestandar;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopCondicionesSubestandar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.DashboardGeneral
{
    public class DashboardGeneralController : Controller
    {
        private readonly IInspeccionesActoCondicionBP _inspeccionesActoCondicionBP;
        private readonly IInspeccionesEstadosCondicion _inspeccionesEstadosCondicion;
        private readonly IRankingGerenciaObsVencidas _rankingGerenciaObsVencidas;
        private readonly IRankingGerenciaReportante _rankingGerenciaReportante;
        private readonly IRankingUsuariosIngresoInspecciones _rankingUsuariosIngresoInspecciones;
        private readonly IRankingUsuariosObsVencidas _rankingUsuariosObsVencidas;
        private readonly ITopActosSubestandar _topActosSubestandar;
        private readonly ITopCondicionesSubestandar _topCondicionesSubestandar;

        public DashboardGeneralController(
            IInspeccionesActoCondicionBP inspeccionesActoCondicionBP, 
            IInspeccionesEstadosCondicion inspeccionesEstadosCondicion,
            IRankingGerenciaObsVencidas rankingGerenciaObsVencidas,
            IRankingGerenciaReportante rankingGerenciaReportante, 
            IRankingUsuariosIngresoInspecciones rankingUsuariosIngresoInspecciones,
            IRankingUsuariosObsVencidas rankingUsuariosObsVencidas,
            ITopActosSubestandar topActosSubestandar,
            ITopCondicionesSubestandar topCondicionesSubestandar)
        {
            _inspeccionesActoCondicionBP = inspeccionesActoCondicionBP;
            _inspeccionesEstadosCondicion = inspeccionesEstadosCondicion;
            _rankingGerenciaObsVencidas = rankingGerenciaObsVencidas;
            _rankingGerenciaReportante = rankingGerenciaReportante;
            _rankingUsuariosIngresoInspecciones = rankingUsuariosIngresoInspecciones;
            _rankingUsuariosObsVencidas = rankingUsuariosObsVencidas;
            _topActosSubestandar = topActosSubestandar;
            _topCondicionesSubestandar = topCondicionesSubestandar;
        }

        public async Task<IActionResult> Test()
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

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> InspeccionesActoCondicionBP()
        {
            var data = await _inspeccionesActoCondicionBP.Execute();
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> InspeccionesEstadosCondicion()
        {
            var data = await _inspeccionesEstadosCondicion.Execute();
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingGerenciaObsVencidas()
        {
            var data = await _rankingGerenciaObsVencidas.Execute(10);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingGerenciaReportante()
        {
            var data = await _rankingGerenciaReportante.Execute(10);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingUsuariosIngresoInspecciones()
        {
            var data = await _rankingUsuariosIngresoInspecciones.Execute(10);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> RankingUsuariosObsVencidas()
        {
            var data = await _rankingUsuariosObsVencidas.Execute(10);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> TopActosSubestandar()
        {
            var data = await _topActosSubestandar.Execute(10);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> TopCondicionesSubestandar()
        {
            var data = await _topCondicionesSubestandar.Execute(10);
            dynamic response = new
            {
                data = data
            };
            return Ok(response);
        }
    }
}
