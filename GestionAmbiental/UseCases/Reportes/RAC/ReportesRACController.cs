using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetDetalleReportesXEstado;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetItemsFiltroResumen;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetNReportesEntreFechas;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetObservacionesXEstado;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesNivelRiesgoMA;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesNivelRiesgoSSO;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesTipoCondicionxNivelRiesgoMA;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesTipoCondicionxNivelRiesgoSSO;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesTipoInspeccionMA;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesTipoInspeccionSSO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.RAC
{
    public class ReportesRACController : Controller
    {
        private readonly IGetObservacionesXEstado _getObservacionesXEstado;
        private readonly IGetItemsFiltroResumen _getItemsFiltroResumen;
        private readonly IGetNReportesEntreFechas _getNReportesEntreFechas;
        private readonly IGetDetalleReportesXEstado _getDetalleReportesXEstado;
        private readonly IGetReportesNivelRiesgoMA _getReportesNivelRiesgoMA;
        private readonly IGetReportesNivelRiesgoSSO _getReportesNivelRiesgoSSO;
        private readonly IGetReportesClasificacionMA _getReportesClasificacionMA;
        private readonly IGetReportesClasificacionSSO _getReportesClasificacionSSO;
        private readonly IGetReportesTipoCondicionxNivelRiesgoMA _getReportesTipoCondicionxNivelRiesgoMA;
        private readonly IGetReportesTipoCondicionxNivelRiesgoSSO _getReportesTipoCondicionxNivelRiesgoSSO;
        private readonly IDataProtector _protector;
        public ReportesRACController(
            IGetObservacionesXEstado getObservacionesXEstado,
            IGetDetalleReportesXEstado getDetalleReportesXEstado,
            IGetItemsFiltroResumen getItemsFiltroResumen,
            IGetNReportesEntreFechas getNReportesEntreFechas,
            IGetReportesNivelRiesgoMA getReportesNivelRiesgoMA,
            IGetReportesNivelRiesgoSSO getReportesNivelRiesgoSSO,
            IGetReportesClasificacionMA getReportesClasificacionMA,
            IGetReportesClasificacionSSO getReportesClasificacionSSO,
            IGetReportesTipoCondicionxNivelRiesgoMA getReportesTipoCondicionxNivelRiesgoMA,
            IGetReportesTipoCondicionxNivelRiesgoSSO getReportesTipoCondicionxNivelRiesgoSSO,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings
            )
        {
            _getObservacionesXEstado = getObservacionesXEstado;
            _getDetalleReportesXEstado = getDetalleReportesXEstado;
            _getItemsFiltroResumen = getItemsFiltroResumen;
            _getNReportesEntreFechas = getNReportesEntreFechas;
            _getReportesNivelRiesgoMA = getReportesNivelRiesgoMA;
            _getReportesNivelRiesgoSSO = getReportesNivelRiesgoSSO;
            _getReportesClasificacionMA = getReportesClasificacionMA;
            _getReportesClasificacionSSO = getReportesClasificacionSSO;
            _getReportesTipoCondicionxNivelRiesgoMA = getReportesTipoCondicionxNivelRiesgoMA;
            _getReportesTipoCondicionxNivelRiesgoSSO = getReportesTipoCondicionxNivelRiesgoSSO;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.RACIdRouteValue);
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
                var model = new FiltroReportesRACModel();

                var selectItemsResumen = await _getItemsFiltroResumen.Execute();

                var filtroGerencias = selectItemsResumen.Gerencias.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                filtroGerencias.Insert(0, new SelectListItem("Todo", "0", true));

                var filtroEmpresas = selectItemsResumen.Empresas.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                filtroEmpresas.Insert(0, new SelectListItem("Todo", "0", true));

                var filtroUsuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                filtroUsuarios.Insert(0, new SelectListItem("Todo", "0", true));

                ViewBag.ItemsGerencia = filtroGerencias;
                ViewBag.ItemsEmpresa = filtroEmpresas;
                ViewBag.ItemsUsuario = filtroUsuarios;
                ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
                ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Filtrar(int IdGerencia,int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nObservaciones = await _getObservacionesXEstado.Execute(IdGerencia, IdEmpresa, IdUsuario,FechaInicio,FechaFin);

            dynamic response = new
            {
                nObservaciones = nObservaciones,
                /*
                fInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"),
                fFin = DateTime.Now.ToString("yyyy-MM-dd")
                */
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetNReportesEntreFechas(int IdGerencia, int IdEmpresa, int IdUsuario,string FechaInicio,string FechaFin)
        {
            var nReportesEntreFechas = await _getNReportesEntreFechas.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                nReportesEntreFechas = nReportesEntreFechas
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetNReportesXNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            var nRepotesNivelRiesgoMA = await _getReportesNivelRiesgoMA.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                NR_MA = nRepotesNivelRiesgoMA
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetNReportesXNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            var nRepotesNivelRiesgoSSO = await _getReportesNivelRiesgoSSO.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                NR_SSO = nRepotesNivelRiesgoSSO
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetNReportesXClasificacionMA(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            var nRepotesClasificacionMA = await _getReportesClasificacionMA.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                C_MA = nRepotesClasificacionMA
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetNReportesXClasificacionSSO(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            var nRepotesClasificacionSSO = await _getReportesClasificacionSSO.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                C_SSO = nRepotesClasificacionSSO
            };
            return Ok(response);
         
        }
        public async Task<IActionResult> GetReportesTipoCondicionxNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            var nReportesMA_TipoCondicion = await _getReportesTipoCondicionxNivelRiesgoMA.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                NR_TC_MA = nReportesMA_TipoCondicion
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetReportesTipoCondicionxNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            var nReportesSSO_TipoCondicion = await _getReportesTipoCondicionxNivelRiesgoSSO.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                NR_TC_SSO = nReportesSSO_TipoCondicion
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetDetalleReporteXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin,string estado)
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
                var data = await _getDetalleReportesXEstado.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin, estado);
                foreach (var item in data.racs)
                {
                    item.Id_Rac_Encriptado = _protector.Protect(item.Id_RAC.ToString());
                }
                ViewBag.nReportes = data.nRacs;
                ViewBag.Estado = estado;
                ViewBag.Data = data.racs;
                return View();
            }
        }
    }
}