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
        private readonly Application.UseCases.ResiduosSolidos.GetSelectItems.IGetSelectItems _getItemsFiltrosIngresoSalida;

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
            Application.UseCases.ResiduosSolidos.GetSelectItems.IGetSelectItems getItemsFiltrosIngresoSalida,
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
            _getItemsFiltrosIngresoSalida = getItemsFiltrosIngresoSalida;
            _descargarRegistros = descargarRegistros;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Dashboard()
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

                var itemsFiltroInspecciones = await _getItemsFiltroInspecciones.Execute();
                var itemsFiltroResiduos = await _getItemsFiltrosIngresoSalida.Execute();

                var filtroEmpresas = itemsFiltroInspecciones.Empresas.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                filtroEmpresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var filtroGerencias = itemsFiltroInspecciones.Gerencias.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var filtroRespLevObs = itemsFiltroInspecciones.RespLevObs.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                filtroRespLevObs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var grupoMA = new SelectListGroup() { Name = "Asesores MA", Disabled = false };
                var grupoSSO = new SelectListGroup() { Name = "Asesores SSO", Disabled = false };

                var filtroAsesor = itemsFiltroInspecciones.AsesoresMAP.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos, Group = grupoMA }).ToList();
                filtroAsesor.AddRange(itemsFiltroInspecciones.AsesoresSSO.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos, Group = grupoSSO }).ToList());
                filtroAsesor.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var filtroACR_CMRS = itemsFiltroResiduos.ItemsACR_CMRS.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_ACR_CMRS.ToString(), Text = g.Descripcion }).ToList();
                filtroACR_CMRS.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var filtroIngresoSalida = itemsFiltroResiduos.ItemsIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Ingreso_Salida.ToString(), Text = g.Descripcion }).ToList();
                filtroIngresoSalida.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var filtroMotivo = itemsFiltroResiduos.ItemsMotivoIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Motivo.ToString(), Text = g.Descripcion }).ToList();
                filtroMotivo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var filtroTipoResiduo = itemsFiltroResiduos.ItemsTipoResiduo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Residuo.ToString(), Text = g.Descripcion }).ToList();
                filtroTipoResiduo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var filtroResiduosPeligrosos = itemsFiltroResiduos.ItemsResiduoSolido.Where(x=>x.Tipo_Residuo.Descripcion.Equals("Peligroso")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                var filtroResiduosNoPeligrosos = itemsFiltroResiduos.ItemsResiduoSolido.Where(x=>x.Tipo_Residuo.Descripcion.Equals("No Peligroso")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
               
                var filtroEstados = new List<SelectListItem> {
                    new SelectListItem("--SELECCIONE--", "", true),
                    new SelectListItem("Pendiente de Implementación", "Pendiente", false),
                    new SelectListItem("Pendiente de Verificación", "Verificacion", false),
                    new SelectListItem("Vencido", "Vencido", false),
                    new SelectListItem("Aprobado", "Aprobado", false),
                    new SelectListItem("Rechazado (Buenas Prácticas)", "Rechazado", false)
                };

                //GENERAL
                ViewBag.FiltroEmpresas = filtroEmpresas;
                ViewBag.FiltroGerencias = filtroGerencias;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");
                ViewBag.FechaInicioRegistros = new DateTime(DateTime.Now.Year,1,1).ToString("yyyy-MM-dd");
                ViewBag.FechaFinRegistros = DateTime.Now.ToString("yyyy-MM-dd");
                //INSPECCIONES Y RAC
                ViewBag.FiltroRespLevObs = filtroRespLevObs;
                ViewBag.FiltroAsesor = filtroAsesor;
                ViewBag.FiltroEstados = filtroEstados;
                //INGRESO Y SALIDA RESIDUOS SOLIDOS
                ViewBag.FiltroACR_CMRS = filtroACR_CMRS;
                ViewBag.FiltroIngresoSalida = filtroIngresoSalida;
                ViewBag.FiltroMotivo = filtroMotivo;
                ViewBag.FiltroTipoResiduo = filtroTipoResiduo;
                ViewBag.FiltroResiduosPeligrosos = filtroResiduosPeligrosos;
                ViewBag.FiltroResiduosNoPeligrosos = filtroResiduosNoPeligrosos;
                
                return View();
            }
        }
        public async Task<IActionResult> DescargarRegistros(string sTime, string eTime)
        {
            string templatePath = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Dashboard");
            var ms = await _descargarRegistros.DescargarInformacion(sTime, eTime, templatePath);
            ms.Position = 0;

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegistrosGestiona-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }
    }
}
