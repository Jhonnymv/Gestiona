using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetBPMAPvsSSO;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetBPMAPvsSSO_Estado;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetDetalleObsxEstado;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetInspeccionesXFecha;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetItemsFiltro;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetNInspecciones;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetNObservaciones;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsClasificacionMA;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsClasificacionSSO;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXFecha;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsMAPvsSSO;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsMAPvsSSO_Estado;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsNivelRiesgoMA;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsNivelRiesgoSSO;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsTipoCondicionxNivelRiesgoMA;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsTipoCondicionxNivelRiesgoSSO;
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
        private readonly IGetNObservaciones _getNObservaciones;
        private readonly IGetObsMAPvsSSO _getObsMAPvsSSO;
        private readonly IGetObsMAPvsSSO_Estado _getObsMAPvsSSO_Estado;
        private readonly IGetBPMAPvsSSO _getBPMAPvsSSO;
        private readonly IGetBPMAPvsSSO_Estado _getBPMAPvsSSO_Estado;
        private readonly IGetObsNivelRiesgoMA _getObsNivelRiesgoMA;
        private readonly IGetObsNivelRiesgoSSO _getObsNivelRiesgoSSO;
        private readonly IGetObsClasificacionMA _getObsClasificacionMA;
        private readonly IGetObsClasificacionSSO _getObsClasificacionSSO;
        private readonly IGetObsTipoCondicionxNivelRiesgoMA _getObsTipoCondicionxNivelRiesgoMA;
        private readonly IGetObsTipoCondicionxNivelRiesgoSSO _getObsTipoCondicionxNivelRiesgoSSO;
        private readonly IGetDetalleObsxEstado _getDetalleObsxEstado;

        private readonly IDataProtector _protector;

        public ReportesInspeccionesController(
            IGetItemsFiltro getItemsFiltro,
            IGetNInspecciones getNInspecciones,
            IGetInspeccionesXFecha getInspeccionesXFecha,
            IGetObservacionesXFecha getObservacionesXFecha,
            IGetNObservaciones getNObservaciones,
            IGetObsMAPvsSSO getObsMAPvsSSO,
            IGetObsMAPvsSSO_Estado getObsMAPvsSSO_Estado,
            IGetBPMAPvsSSO getBPMAPvsSSO,
            IGetBPMAPvsSSO_Estado getBPMAPvsSSO_Estado,
            IGetObsNivelRiesgoMA getObsNivelRiesgoMA,
            IGetObsNivelRiesgoSSO getObsNivelRiesgoSSO,
            IGetObsClasificacionMA getObsClasificacionMA,
            IGetObsClasificacionSSO getObsClasificacionSSO,
            IGetObsTipoCondicionxNivelRiesgoMA getObsTipoCondicionxNivelRiesgoMA,
            IGetObsTipoCondicionxNivelRiesgoSSO getObsTipoCondicionxNivelRiesgoSSO,
            IGetDetalleObsxEstado getDetalleObsxEstado,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getItemsFiltro = getItemsFiltro;
            _getNInspecciones = getNInspecciones;
            _getInspeccionesXFecha = getInspeccionesXFecha;
            _getObservacionesXFecha = getObservacionesXFecha;
            _getNObservaciones = getNObservaciones;
            _getObsMAPvsSSO = getObsMAPvsSSO;
            _getObsMAPvsSSO_Estado = getObsMAPvsSSO_Estado;
            _getBPMAPvsSSO = getBPMAPvsSSO;
            _getBPMAPvsSSO_Estado = getBPMAPvsSSO_Estado;
            _getObsNivelRiesgoMA = getObsNivelRiesgoMA;
            _getObsNivelRiesgoSSO = getObsNivelRiesgoSSO;
            _getObsClasificacionMA = getObsClasificacionMA;
            _getObsClasificacionSSO = getObsClasificacionSSO;
            _getObsTipoCondicionxNivelRiesgoMA = getObsTipoCondicionxNivelRiesgoMA;
            _getObsTipoCondicionxNivelRiesgoSSO = getObsTipoCondicionxNivelRiesgoSSO;
            _getDetalleObsxEstado = getDetalleObsxEstado;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
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

                var itemsFiltro = await _getItemsFiltro.Execute();

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
        public async Task<IActionResult> ActualizarContadorInspecciones(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin)
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
        public async Task<IActionResult> ActualizarObservacionesXFecha(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string frecuencia, string estado)
        {
            var observacionesXFecha = await _getObservacionesXFecha.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, frecuencia, estado);
            dynamic response = new
            {
                observacionesXFecha = observacionesXFecha
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarContadorObservaciones(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var nObservaciones = await _getNObservaciones.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                nObservaciones = nObservaciones
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsMAPvsSSO(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var mapvssso = await _getObsMAPvsSSO.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                mapvssso = mapvssso
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsMAPvsSSO_x_Estado(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var mapvssso = await _getObsMAPvsSSO_Estado.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                mapvssso = mapvssso
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarBPMAPvsSSO(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var mapvssso = await _getBPMAPvsSSO.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                mapvssso = mapvssso
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarBPMAPvsSSO_x_Estado(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var mapvssso = await _getBPMAPvsSSO_Estado.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                mapvssso = mapvssso
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var nivelRiesgoMA = await _getObsNivelRiesgoMA.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                nivelRiesgoMA = nivelRiesgoMA
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var nivelRiesgoSSO = await _getObsNivelRiesgoSSO.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                nivelRiesgoSSO = nivelRiesgoSSO
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsClasificacionMA(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var clasificacionMA = await _getObsClasificacionMA.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                clasificacionMA = clasificacionMA
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsClasificacionSSO(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var clasificacionSSO = await _getObsClasificacionSSO.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                clasificacionSSO = clasificacionSSO
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsTipoCondicionxNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var tipoNivelRiesgoMA = await _getObsTipoCondicionxNivelRiesgoMA.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                tipoNivelRiesgoMA = tipoNivelRiesgoMA
            };
            return Ok(response);
        }
        public async Task<IActionResult> ActualizarObsTipoCondicionxNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
        {
            var tipoNivelRiesgoSSO = await _getObsTipoCondicionxNivelRiesgoSSO.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            dynamic response = new
            {
                tipoNivelRiesgoSSO = tipoNivelRiesgoSSO
            };
            return Ok(response);
        }
        public IActionResult DetalleObservacionEstado(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado)
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
                ViewBag.Empresa = empresa;
                ViewBag.Gerencia = gerencia;
                ViewBag.Responsable = respLevObs;
                ViewBag.Asesor = asesor;
                ViewBag.FechaInicio = FechaInicio;
                ViewBag.FechaFin = FechaFin;
                ViewBag.Estado = estado;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaObservacionesxEstado(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado,int pag,int rpp)
        {
            var data = await _getDetalleObsxEstado.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado);
            data = data.OrderByDescending(x => x.Codigo).ToList();
            var nObs = data.Count();
            int nPaginas = nObs % rpp != 0 ? (nObs / rpp) + 1 : nObs / rpp;
            data = data.Skip((pag - 1) * rpp).Take(rpp).ToList();
           

            var tabla = "";
            var paginador = "";
            if (nObs > 0)
            {
                foreach(var item in data)
                {
                    item.Id_Cabecera_Encriptada = _protector.Protect(item.Id_Cabecera.ToString());

                    tabla += "<tr>";
                    tabla += "<td><a href = \"/Inspecciones/EditarInspeccion?IdCabecera="+item.Id_Cabecera_Encriptada+ "\">" + item.Codigo + "</a></td>";
                    tabla += "<td>" + item.Empresa + "</td>";
                    tabla += "<td>" + item.Gerencia + "</td>";
                    tabla += "<td>" + item.Fecha_Inspeccion + "</td>";
                    tabla += "<td>" + item.Tipo_Observacion + "</td>";
                    tabla += "<td>" + item.Asesor + "</td>";
                    tabla += "<td>" + item.Tipo_Condicion + "</td>";
                    switch (item.Nivel_Riesgo)
                    {
                        case "Tolerable": tabla += "<td><span class=\"badge bg-success\">" + item.Nivel_Riesgo + "</span></td>"; break;
                        case "Alerta": tabla += "<td><span class=\"badge bg-warning\">" + item.Nivel_Riesgo + "</span></td>"; break;
                        case "No Aceptable": tabla += "<td><span class=\"badge bg-danger\">" + item.Nivel_Riesgo + "</span></td>"; break;
                        case "Bajo": tabla += "<td><span class=\"badge bg-success\">" + item.Nivel_Riesgo + "</span></td>"; break;
                        case "Medio": tabla += "<td><span class=\"badge bg-warning\">" + item.Nivel_Riesgo + "</span></td>"; break;
                        case "Alto": tabla += "<td><span class=\"badge bg-danger\">" + item.Nivel_Riesgo + "</span></td>"; break;
                        default: tabla += "<td><span class=\"badge bg-secondary\">" + item.Nivel_Riesgo + "</span>"; break;
                    }
                    tabla += "<td>" + item.Responsable + "</td>";
                    tabla += "<td>" + item.Fecha_Programada + "</td>";
                    tabla += "</tr>";
                }
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pag >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pag - 2; i <= pag + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pag)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pag <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";
            }
            dynamic response = new
            {
                data = tabla,
                paginador = paginador
            };
            return Ok(response);
        }
    }
}