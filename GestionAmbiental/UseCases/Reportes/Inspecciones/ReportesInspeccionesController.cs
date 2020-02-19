using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetInspeccionesXFecha;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetItemsFiltro;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetNInspecciones;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXFecha;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Reportes.Inspecciones
{
    public class ReportesInspeccionesController : Controller
    {
        private readonly IGetItemsFiltro _getItemsFiltro;
        private readonly IGetNInspecciones _getNInspecciones;
        private readonly IGetInspeccionesXFecha _getInspeccionesXFecha;
        private readonly IGetObservacionesXFecha _getObservacionesXFecha;


        private readonly IDataProtector _protector;

        public ReportesInspeccionesController(
            IGetItemsFiltro getItemsFiltro,
            IGetNInspecciones getNInspecciones,
            IGetInspeccionesXFecha getInspeccionesXFecha, 
            IGetObservacionesXFecha getObservacionesXFecha,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getItemsFiltro = getItemsFiltro;
            _getNInspecciones = getNInspecciones;
            _getInspeccionesXFecha = getInspeccionesXFecha;
            _getObservacionesXFecha = getObservacionesXFecha;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
        }

        public async Task<IActionResult> Resumen()
        {
            /*if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));*/
            var userInfo = new
            {
                NombreUsuario = "kevin",
                Rol = "AsesorMA"
            };

            ViewBag.Rol = userInfo.Rol.ToString();
            ViewBag.Usuario = userInfo.NombreUsuario;

            var itemsFiltro = await _getItemsFiltro.Execute();

            var filtroEmpresas = itemsFiltro.Empresas.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
            filtroEmpresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var filtroGerencias = itemsFiltro.Gerencias.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
            filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var filtroRespLevObs = itemsFiltro.RespLevObs.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            filtroRespLevObs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var grupoMA = new SelectListGroup() { Name = "Asesores MA", Disabled = false };
            var grupoSSO = new SelectListGroup() { Name = "Asesores SSO", Disabled = false };

            var filtroAsesor = itemsFiltro.AsesoresMAP.OrderBy(x=>x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos, Group = grupoMA}).ToList();
            filtroAsesor.AddRange(itemsFiltro.AsesoresSSO.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos, Group = grupoSSO }).ToList());
            filtroAsesor.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.FiltroEmpresas = filtroEmpresas;
            ViewBag.FiltroGerencias = filtroGerencias;
            ViewBag.FiltroRespLevObs = filtroRespLevObs;
            ViewBag.FiltroAsesor = filtroAsesor;
            ViewBag.FechaInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            ViewBag.FechaFin = DateTime.Now.ToString("yyyy-MM-dd");

            return View();
           // }
        }
        public async Task<IActionResult> ActualizarContadorInspecciones(int empresa,int gerencia,int respLevObs, int asesor, string FechaInicio,string FechaFin)
        {
            var nInspecciones = await _getNInspecciones.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin);
            dynamic response = new
            {
                nInspecciones = nInspecciones
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarInspeccionesXFecha(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string frecuencia)
        {
            var inspeccionesXFecha = await _getInspeccionesXFecha.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, frecuencia);
            dynamic response = new
            {
                inspeccionesXFecha = inspeccionesXFecha
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObservacionesXFecha(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string frecuencia,string estado)
        {
            var inspeccionesXFecha = await _getObservacionesXFecha.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, frecuencia,estado);
            dynamic response = new
            {
                inspeccionesXFecha = inspeccionesXFecha
            };
            return Ok(response);
        }

    }
}