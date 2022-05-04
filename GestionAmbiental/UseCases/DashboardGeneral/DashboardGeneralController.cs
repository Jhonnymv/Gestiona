using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.InspeccionesActoCondicionBP;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.InspeccionesEstadosCondicion;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingGerenciaReportante;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingUsuariosIngresoInspecciones;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.RankingUsuariosObsVencidas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopActosSubestandar;
using GestionAmbiental.Application.UseCases.DashboardGeneral.Inspecciones.TopCondicionesSubestandar;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetItemsFiltro;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private DescargarRegistros _descargarRegistros;
        private IWebHostEnvironment _hostingEnvironment;

        private readonly IGetItemsFiltro _getItemsFiltroInspecciones;

        public DashboardGeneralController(
            IInspeccionesActoCondicionBP inspeccionesActoCondicionBP, 
            IInspeccionesEstadosCondicion inspeccionesEstadosCondicion,
            IRankingGerenciaObsVencidas rankingGerenciaObsVencidas,
            IRankingGerenciaReportante rankingGerenciaReportante, 
            IRankingUsuariosIngresoInspecciones rankingUsuariosIngresoInspecciones,
            IRankingUsuariosObsVencidas rankingUsuariosObsVencidas,
            ITopActosSubestandar topActosSubestandar,
            ITopCondicionesSubestandar topCondicionesSubestandar,
            IGetItemsFiltro getItemsFiltroInspecciones,
            DescargarRegistros descargarRegistros,
            IWebHostEnvironment hostingEnvironment)
        {
            _inspeccionesActoCondicionBP = inspeccionesActoCondicionBP;
            _inspeccionesEstadosCondicion = inspeccionesEstadosCondicion;
            _rankingGerenciaObsVencidas = rankingGerenciaObsVencidas;
            _rankingGerenciaReportante = rankingGerenciaReportante;
            _rankingUsuariosIngresoInspecciones = rankingUsuariosIngresoInspecciones;
            _rankingUsuariosObsVencidas = rankingUsuariosObsVencidas;
            _topActosSubestandar = topActosSubestandar;
            _topCondicionesSubestandar = topCondicionesSubestandar;
            _getItemsFiltroInspecciones = getItemsFiltroInspecciones;
            _descargarRegistros = descargarRegistros;
            _hostingEnvironment = hostingEnvironment;
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

                var itemsFiltro = await _getItemsFiltroInspecciones.Execute();

                var filtroEmpresas = itemsFiltro.Empresas.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                filtroEmpresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var filtroGerencias = itemsFiltro.Gerencias.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var filtroRespLevObs = itemsFiltro.RespLevObs.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                filtroRespLevObs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var grupoMA = new SelectListGroup() { Name = "Asesores MA", Disabled = false };
                var grupoSSO = new SelectListGroup() { Name = "Asesores SSO", Disabled = false };

                var filtroAsesor = itemsFiltro.AsesoresMAP.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos, Group = grupoMA }).ToList();
                filtroAsesor.AddRange(itemsFiltro.AsesoresSSO.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos, Group = grupoSSO }).ToList());
                filtroAsesor.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var filtroEstados = new List<SelectListItem> {
                    new SelectListItem("--SELECCIONE--", "", true),
                    new SelectListItem("Pendiente de Implementación", "Pendiente", false),
                    new SelectListItem("Pendiente de Verificación", "Verificacion", false),
                    new SelectListItem("Vencido", "Vencido", false),
                    new SelectListItem("Aprobado", "Aprobado", false),
                    new SelectListItem("Rechazado (Buenas Prácticas)", "Rechazado", false)
                };

                ViewBag.FiltroEmpresas = filtroEmpresas;
                ViewBag.FiltroGerencias = filtroGerencias;
                ViewBag.FiltroRespLevObs = filtroRespLevObs;
                ViewBag.FiltroAsesor = filtroAsesor;
                ViewBag.FiltroEstados = filtroEstados;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

                return View();
            }
        }
        public async Task<IActionResult> DescargarRegistros()
        {
            string templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Dashboard");
            var ms = await _descargarRegistros.DescargarInformacion("01/01/2021", "03/05/2022",templatePath);
            ms.Position = 0;

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegistrosGestiona-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }
    }
}
