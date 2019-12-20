using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarDetalles;
using GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.VerificarObservacion;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.VerificacionMA
{
    public class InspeccionesVerificacionMAController : Controller
    {
        private IMemoryCache _cache;
        private readonly ISelectItems _selectItems;
        private readonly IConsultarCabecerasVerificacion _consultarCabecerasVerificacion;
        private readonly IConsultarCabeceraVerificacion _consultarCabeceraVerificacion;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IConsultarDetallesVerificacion _consultarDetalleVerificacion;
        private readonly IVerificarObservacionMA _verificarObservacion;
        private readonly IDataProtector _protector;

        private readonly IHostingEnvironment _hostingEnvironment;

        private const string CarpetaEvidenciasCondicion = "ftp://127.0.0.1:2020/InspeccionesAmbientales/EvidenciasRegistro/";
        private const string CarpetaEvidenciasImplementacion = "ftp://127.0.0.1:2020/InspeccionesAmbientales/EvidenciasImplementacion/";

        public InspeccionesVerificacionMAController(
            IMemoryCache cache,
            ISelectItems selectItems,
            IConsultarCabecerasVerificacion consultarCabecerasVerificacion, 
            IConsultarCabeceraVerificacion consultarCabeceraVerificacion, 
            IConsultarInspeccionDetalle consultarInspeccionDetalle, 
            IConsultarDetallesVerificacion consultarDetalleVerificacion,
            IVerificarObservacionMA verificarObservacion,
            IHostingEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _cache = cache;
            _selectItems = selectItems;
            _consultarCabecerasVerificacion = consultarCabecerasVerificacion;
            _consultarCabeceraVerificacion = consultarCabeceraVerificacion;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarDetalleVerificacion = consultarDetalleVerificacion;
            _verificarObservacion = verificarObservacion;
            _hostingEnvironment = hostingEnvironment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
        }

        public async Task<IActionResult> Resumen()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var selectItemsResumen = await _selectItems.ItemVistaResumen();

                var filtroGerencias = selectItemsResumen.GerenciaEjecutora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();

                filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Filtro = filtroGerencias;

                return View();
            }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, int filtroGerencia, string filtroCodigo, int pagina, int rpp)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                DateTime fecha;
                if (filtroFecha != null)
                    fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
                else
                    fecha = (DateTime)SqlDateTime.MinValue;

                filtroCodigo = filtroCodigo ?? "";
                var datosTabla = await _consultarCabecerasVerificacion.Execute(pagina, rpp, fecha, filtroGerencia, filtroCodigo);
                var filasTabla = "";
                var paginador = "";
                int nPaginas = datosTabla.NCabeceras % rpp != 0 ? (datosTabla.NCabeceras / rpp) + 1 : datosTabla.NCabeceras / rpp;
                if (datosTabla.NCabeceras > 0)
                {
                    foreach (var fila in datosTabla.Cabeceras)
                    {
                        var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                        filasTabla += "<tr>";
                        /*switch (fila.Estado)
                        {
                            case "Pendiente": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/alert-icon.png\"></td>"; break;
                            case "Vencido": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/pending-icon.png\"></td>"; break;
                            default: filasTabla += "<td class=\"alert-column\"></td>"; break;
                        }*/
                        filasTabla += "<td><a href=\"/InspeccionesVerificacionMA/VerificarInspeccion?IdCabecera=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                        filasTabla += "<td>" + fila.Gerencia + "</td>";
                        filasTabla += "<td>" + fila.Empresa + "</td>";
                        filasTabla += "<td>" + fila.AreaInspeccionada + "</td>";
                        filasTabla += "<td>" + fila.FechaInspeccion.ToString("dd/MM/yyyy HH:mm") + "</td>";
                        filasTabla += "<td>" + fila.NObservaciones + "</td>";
                        filasTabla += "</tr>";
                    }
                    paginador += "<ul class=\"pagination\">";
                    for (int i = 1; i <= nPaginas; i++)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
                    paginador += "</ul>";
                }
                else
                {
                    filasTabla = "Sin datos";
                }
                dynamic response = new
                {
                    Data = filasTabla,
                    Paginador = paginador
                };
                return Ok(response);
            }
        }
        public async Task<IActionResult> VerificarInspeccion(string IdCabecera)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (IdCabecera.Length != 0)
                {
                    int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdCabecera));
                    var cabecera = await _consultarCabeceraVerificacion.Execute(IdDesencriptado);
                    ViewBag.Cabecera = cabecera;
                    var filaObservacion = "";
                    var observaciones = await _consultarInspeccionDetalle.ExecuteVerificacion(IdDesencriptado, User.Identity.Name);
                    foreach (var observacion in observaciones)
                    {
                        var IdObsEncriptado = _protector.Protect(observacion.IdInspeccionDetalle.ToString());
                        //if (observacion.Estado.Equals(EstadosInspeccion.Implementado.ToString()))
                        //{
                        var evidencia = "<label style=\"width:110px\">";
                        var evidencia_implementacion = "<label style=\"width:110px\">";
                        for (int i = 0; i < observacion.EvidenciaCondicion.Count; i++)
                            evidencia = "<a href=\"/Inspecciones/Download?fileName=" + observacion.EvidenciaCondicion[i] + "&tipo=condicion\">Evidencia</a>";
                        evidencia += "</label>";
                        for (int i = 0; i < observacion.EvidenciaImplementacion.Count; i++)
                        {
                            evidencia_implementacion = "<a href=\"/InspeccionesImplementacion/Download?fileName=" + observacion.EvidenciaImplementacion[i] + "&tipo=implementacion\">Evidencia</a>";
                        }
                        evidencia_implementacion += "</label>";
                        filaObservacion += "<tr>";
                        filaObservacion += "<td style=\"display: none; \">" + observacion.IdInspeccionDetalle + "</td>";
                        filaObservacion += "<td>" + observacion.TipoInspeccion + "</td>";
                        filaObservacion += "<td>" + observacion.AsesorMASeguridad + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionCondicion + "</td>";
                        filaObservacion += "<td><label style=\"width:250px\">" + observacion.TipoCondicion + "</label></td>";
                        filaObservacion += "<td>" + evidencia + "</td>";
                        switch (observacion.NivelRiesgo)
                        {
                            case "Tolerable": filaObservacion += "<td><span class=\"badge badge-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alerta": filaObservacion += "<td><span class=\"badge badge-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "No Aceptable": filaObservacion += "<td><span class=\"badge badge-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Bajo": filaObservacion += "<td><span class=\"badge badge-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Medio": filaObservacion += "<td><span class=\"badge badge-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alto": filaObservacion += "<td><span class=\"badge badge-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            default: filaObservacion += "<td><span class=\"badge badge-secondary\">" + observacion.NivelRiesgo + "</span>"; break;
                        }
                        filaObservacion += "<td><label style=\"width:200px\">" + observacion.DescripcionAccion + "</label></td>";
                        filaObservacion += "<td>" + observacion.ResponsableMarcobre + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableContratista + "</td>";
                        filaObservacion += "<td>" + observacion.FechaProgramada.ToShortDateString() + "</td>";
                        filaObservacion += "<td>" + evidencia_implementacion + "</td>";
                        filaObservacion += "<td>" + observacion.ComentarioImplementacion + "</td>";
                        if (observacion.EvidenciaImplementacion.Count != 0 && observacion.Estado != EstadosInspeccion.Aprobado.ToString() && observacion.Estado != EstadosInspeccion.Rechazado.ToString() && observacion.Permisos)
                            filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/InspeccionesVerificacionMA/VerificarObservacion?IdObservacion=" + IdObsEncriptado + "\">" + "Verificar" + "</a></td>";
                        else
                            filaObservacion += "<td><button class=\"btn btn-primary\" href=\"/InspeccionesVerificacionMA/VerificarInspeccion?IdCabecera=" + IdObsEncriptado + "\" disabled>" + "Verificar" + "</button></td>";
                        filaObservacion += "</tr>";
                    }
                    //}
                    ViewBag.NObservaciones = observaciones.ToList().Count;
                    ViewBag.ContenidoTabla = filaObservacion;
                }
                return View();
            }
        }
        public async Task<IActionResult> VerificarObservacion(string IdObservacion)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
                var observacion = await _consultarDetalleVerificacion.Execute(IdDesencriptado);
                var model = new VerificarObservacionMAModel();
                model.idObservacion = observacion.IdInspeccionDetalle;
                model.EncryptedId = IdObservacion;
                model.idCabecera = observacion.IdInspeccionCabecera;
                model.TipoInspeccion = observacion.TipoInspeccion;
                model.Asesor_MA_Seguridad = observacion.AsesorMASeguridad;
                model.CondicionIdentificada = observacion.DescripcionCondicion;
                model.TipoCondicion = observacion.TipoCondicion;
                model.Evidencias = observacion.EvidenciaCondicion;
                model.NiveRiesgo = observacion.NivelRiesgo;
                model.AccionTomada = observacion.DescripcionAccion;
                model.ResponsableMarc = observacion.ResponsableMarcobre;
                model.ResponsableCont = observacion.ResponsableContratista;
                model.FechaProgramada = observacion.FechaProgramada.ToShortDateString();
                model.EvidenciasInspeccion = observacion.EvidenciaImplementacion;
                model.ComentarioImplementacion = observacion.ComentarioImplementacion;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> VerificarObservacion(VerificarObservacionMAModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Accion.Equals("Rechazar")) {
                    await _verificarObservacion.Execute(model.idObservacion, model.ComentarioVerificacionMA, EstadosInspeccion.Rechazado.ToString(), User.Identity.Name);
                    return RedirectToAction("NotificarObservacionRechazada","Notificaciones", new { IdObservacion = model.EncryptedId });
                }
                else
                    await _verificarObservacion.Execute(model.idObservacion, model.ComentarioVerificacionMA, EstadosInspeccion.Aprobado.ToString(), User.Identity.Name);
            }
            var IdEncriptado = _protector.Protect(model.idCabecera.ToString());
            return RedirectToAction("VerificarInspeccion", new { IdCabecera = IdEncriptado });
        }
    }
}