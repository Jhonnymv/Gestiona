using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetDetalleObservacionesXEstado;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetInspeccionesDiarias;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesMAPvsSSO;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesMAPXTipoCondicion;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesSSOXTipoCondicion;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXActoCondicionMAP;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXActoCondicionSSO;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXEstado;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXNivelRiesgoMAP;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXNivelRiesgoSSO;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXUsuario;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetResumenObservacionesXUsuario;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.Inspecciones
{
    public class ReportesInspeccionesController : Controller
    {
        private readonly IGetObservacionesMAPvsSSO _getInspeccionesMAPvsSSO;
        private readonly IGetObservacionesXUsuario _getObservacionesXUsuario;
        private readonly IGetObservacionesXEstado _getObservacionesXEstado;
        private readonly IGetResumenObservacionesXUsuario _getResumenObservacionesXUsuario;
        private readonly IGetInspeccionesDiarias _getInspeccionesDiarias;
        private readonly IGetObservacionesXNivelRiesgoMAP _getObservacionesXNivelRiesgoMAP;
        private readonly IGetObservacionesXNivelRiesgoSSO _getObservacionesXNivelRiesgoSSO;
        private readonly IGetObservacionesXActoCondicionMAP _getObservacionesXActoCondicionMAP;
        private readonly IGetObservacionesXActoCondicionSSO _getObservacionesXActoCondicionSSO;
        private readonly IGetDetalleObservacionesXEstado _getDetalleObservacionesXEstado;
        private readonly IGetObservacionesMAPXTipoCondicion _getObservacionesMAPXTipoCondicion;
        private readonly IGetObservacionesSSOXTipoCondicion _getObservacionesSSOXTipoCondicion;
        private readonly IDataProtector _protector;

        public ReportesInspeccionesController(
            IGetObservacionesMAPvsSSO getInspeccionesMAPvsSSO, 
            IGetObservacionesXUsuario getObservacionesXUsuario, 
            IGetObservacionesXEstado getObservacionesXEstado, 
            IGetResumenObservacionesXUsuario getResumenObservacionesXUsuario,
            IGetInspeccionesDiarias getInspeccionesDiarias, 
            IGetObservacionesXNivelRiesgoMAP getObservacionesXNivelRiesgoMAP, 
            IGetObservacionesXNivelRiesgoSSO getObservacionesXNivelRiesgoSSO, 
            IGetObservacionesXActoCondicionMAP getObservacionesXActoCondicionMAP, 
            IGetObservacionesXActoCondicionSSO getObservacionesXActoCondicionSSO, 
            IGetDetalleObservacionesXEstado getDetalleObservacionesXEstado,
            IGetObservacionesMAPXTipoCondicion getObservacionesMAPXTipoCondicion,
            IGetObservacionesSSOXTipoCondicion getObservacionesSSOXTipoCondicion,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getInspeccionesMAPvsSSO = getInspeccionesMAPvsSSO;
            _getObservacionesXUsuario = getObservacionesXUsuario;
            _getObservacionesXEstado = getObservacionesXEstado;
            _getResumenObservacionesXUsuario = getResumenObservacionesXUsuario;
            _getInspeccionesDiarias = getInspeccionesDiarias;
            _getObservacionesXNivelRiesgoMAP = getObservacionesXNivelRiesgoMAP;
            _getObservacionesXNivelRiesgoSSO = getObservacionesXNivelRiesgoSSO;
            _getObservacionesXActoCondicionMAP = getObservacionesXActoCondicionMAP;
            _getObservacionesXActoCondicionSSO = getObservacionesXActoCondicionSSO;
            _getDetalleObservacionesXEstado = getDetalleObservacionesXEstado;
            _getObservacionesMAPXTipoCondicion = getObservacionesMAPXTipoCondicion;
            _getObservacionesSSOXTipoCondicion = getObservacionesSSOXTipoCondicion;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
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
                var nObservaciones = await _getObservacionesXEstado.Execute();
                var inspecciones_diarias = await _getInspeccionesDiarias.Execute();
                var map_vs_sso = await _getInspeccionesMAPvsSSO.Execute();
                var MAP_x_nivel_riesgo = await _getObservacionesXNivelRiesgoMAP.Execute();
                var SSO_x_nivel_riesgo = await _getObservacionesXNivelRiesgoSSO.Execute();
                var MAP_x_tipo_observacion = await _getObservacionesXActoCondicionMAP.Execute();
                var SSO_x_tipo_observacion = await _getObservacionesXActoCondicionSSO.Execute();


                ViewBag.nObservaciones = nObservaciones.Select(x => x.Valor).ToList();

                var model = new List<ReportesModel>();
                model.Add(new ReportesModel("#Inspecciones registradas en los últimos 30 días", inspecciones_diarias));
                model.Add(new ReportesModel("Cantidad de Observaciones MAP vs SSO", map_vs_sso));
                model.Add(new ReportesModel("Cantidad de Observaciones MAP por Nivel de Riesgo", MAP_x_nivel_riesgo));
                model.Add(new ReportesModel("Cantidad de Observaciones SSO por Nivel de Riesgo", SSO_x_nivel_riesgo));
                model.Add(new ReportesModel("Cantidad de Observaciones MAP por Tipo de Observación", MAP_x_tipo_observacion));
                model.Add(new ReportesModel("Cantidad de Observaciones SSO por Tipo de Observación", SSO_x_tipo_observacion));

                return View(model);
            }
        }
        public async Task<IActionResult> DetalleObservacionEstado(string estado)
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
                var data = await _getDetalleObservacionesXEstado.Execute(estado);
                data = data.OrderByDescending(x => x.Codigo).OrderByDescending(x=>x.FechaProgramada).ToList();
                foreach(var item in data)
                {
                    item.IdInspeccionCabeceraEncriptado = _protector.Protect(item.IdInspeccionCabecera.ToString());
                }
                ViewBag.Data = data;
                ViewBag.nObservaciones = data.Count;
                ViewBag.Estado = estado;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetObservacionesMAPXTipoCondicion()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var data = await _getObservacionesMAPXTipoCondicion.Execute();
                dynamic response = new
                {
                    Data = data
                };
                return Ok(response);
            }

        }
        [HttpPost]
        public async Task<IActionResult> GetObservacionesSSOXTipoCondicion()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var data = await _getObservacionesSSOXTipoCondicion.Execute();
                dynamic response = new
                {
                    Data = data
                };
                return Ok(response);
            }
        }
    }
}