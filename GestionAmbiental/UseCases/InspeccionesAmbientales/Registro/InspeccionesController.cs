using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Inspecciones.AmpliarFechaObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.RegistrarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Registro
{
    public class InspeccionesController : Controller
    {
        private IMemoryCache _cache;

        private readonly IConsultarCabeceras _consultarCabeceras;
        private readonly IConsultarCabecera _consultarCabecera;
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarInspeccionCabecera _registrarInspeccionCabecera;
        private readonly IRegistrarInspeccionObservacion _registrarInspeccionObservacion;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IEditarInspeccionCabecera _editarInspeccionCabecera;
        private readonly IAmpliarFechaObservacion _ampliarFechaObservacion;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;

        public InspeccionesController(
            IMemoryCache cache,
            IConsultarCabeceras consultarCabeceras,
            ISelectItems selectItems,
            IRegistrarInspeccionCabecera registrarInspeccionCabecera,
            IHostingEnvironment hostingEnvironment,
            IRegistrarInspeccionObservacion registrarInspeccionObservacion,
            IConsultarInspeccionDetalle consultarInspeccionDetalle,
            IConsultarCabecera consultarCabecera,
            IEditarInspeccionCabecera editarInspeccionCabecera,
            IAmpliarFechaObservacion ampliarFechaObservacion,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _cache = cache;
            _consultarCabeceras = consultarCabeceras;
            _selectItems = selectItems;
            _registrarInspeccionCabecera = registrarInspeccionCabecera;
            _hostingEnvironment = hostingEnvironment;
            _registrarInspeccionObservacion = registrarInspeccionObservacion;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarCabecera = consultarCabecera;
            _editarInspeccionCabecera = editarInspeccionCabecera;
            _ampliarFechaObservacion = ampliarFechaObservacion;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
            _info_SGA = info_SGA;
        }
        public async Task<IActionResult> Resumen()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var selectItemsResumen = await _selectItems.ItemVistaResumen();

                var filtroGerencias = selectItemsResumen.GerenciaEjecutora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Filtro = filtroGerencias;
                return View();
            }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, int filtroGerencia, string filtroCodigo,int pagina,int rpp)
        {
            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarCabeceras.Execute(pagina, rpp, fecha, filtroGerencia, filtroCodigo);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NCabeceras%rpp!=0 ? (datosTabla.NCabeceras/rpp)+1 : datosTabla.NCabeceras/rpp;
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
                    double avance = fila.NObsImplementadas!=0? Math.Round((float)fila.NObsImplementadas / fila.NObservaciones * 100,1) : 0;
                    filasTabla += "<td><a href=\"/Inspecciones/EditarInspeccion?IdCabecera=" + IdEncriptado + "\">"+fila.Codigo+"</a></td>";
                    filasTabla += "<td>" + fila.Gerencia + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.AreaInspeccionada + "</td>";
                    filasTabla += "<td>" + fila.FechaInspeccion.ToString("dd/MM/yyyy HH:mm") + "</td>";
                    filasTabla += "<td>" + fila.NObservaciones + "</td>";
                    filasTabla += "<td><div class=\"progress\"><div class=\"progress-bar bg-info\" role=\"progressbar\" style=\"width:" + avance+ "%; color: black;font-weight: bold;\" aria-valuemin=\"0\" aria-valuemax=\"100\">" + avance + "%</div></div></td>";
                    filasTabla += "</tr>";
                }
                
                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar("+ i +")\">" + i + "</a></li>";
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
        public async Task<IActionResult> NuevaInspeccion()
        {
            //Cargar observaciones si se tiene
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var selectItemsResumen = await _selectItems.ItemVistaNuevaInspeccion();

                var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                gerenciasEjecutorasMC.RemoveAt(1);

                var empresasMC = selectItemsResumen.EmpContratMarcobre.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var responsables = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposInspeccion = new List<SelectListItem>();
                tiposInspeccion.Insert(0, new SelectListItem("OTRO", "OTRO", false));
                tiposInspeccion.Insert(0, new SelectListItem("NO PLANEADA", "NO PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("PLANEADA", "PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                var idCabecera = Convert.ToInt32(TempData["idCabecera"]);
                if (idCabecera != 0)
                {
                    var filaObservacion = "";
                    var observaciones = await _consultarInspeccionDetalle.Execute(idCabecera,User.Identity.Name);
                    ViewBag.nObservacioness = observaciones.Count;

                    foreach (var observacion in observaciones)
                    {
                        var evidencia = "";
                        for (int i = 0; i < observacion.EvidenciaCondicion.Count; i++)
                        {
                            if (i == 0)
                                evidencia = "<a href=\"" + observacion.EvidenciaCondicion[i] + "\">Evidencia " + (i + 1) + "</a>";
                            else
                                evidencia += "<a href=\"" + observacion.EvidenciaCondicion[i] + "\">, Evidencia " + (i + 1) + "</a>";
                        }
                        filaObservacion += "<tr>";
                        filaObservacion += "<td style=\"display: none; \">" + observacion.IdInspeccionDetalle + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionCondicion + "</td>";
                        filaObservacion += "<td>" + observacion.TipoCondicion + "</td>";
                        filaObservacion += "<td>" + evidencia + "</td>";
                        filaObservacion += "<td>" + observacion.NivelRiesgo + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionAccion + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableMarcobre + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableContratista + "</td>";
                        filaObservacion += "<td>" + observacion.FechaProgramada + "</td>";
                        filaObservacion += "</tr>";

                    }
                    ViewBag.ContenidoTabla = filaObservacion;
                }

                ViewBag.GerenciaMC = gerenciasEjecutorasMC;
                ViewBag.EmpresasMC = empresasMC;
                ViewBag.Responsables = responsables;
                ViewBag.Tipo_Inspeccion = tiposInspeccion;
                return View(new NuevaInspeccionModel());
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevaInspeccion(NuevaInspeccionModel model)
        {

            if (ModelState.IsValid)
            {
                int idCabecera = await _registrarInspeccionCabecera.Execute(
                model.Objetivo, User.Identity.Name, model.Id_Gerencia_Ejecutora, model.Id_Empresa_Contratista, model.Lugar_Inspeccionado, model.Fecha_Inspeccion, model.Hora_Inspeccion,
                model.Tipo_Inspeccion, model.Id_Responsable_Lugar, model.Responsable_Inspeccion, model.Inspectores, model.opcion);
                if (model.opcion.Equals("Observacion")) {
                    var IdEncriptado = _protector.Protect(idCabecera.ToString());

                    return RedirectToAction("NuevaObservacion", new { IdCabecera = IdEncriptado });
                }
            }
                return RedirectToAction("Resumen");
        }
        
        public async Task<IActionResult> NuevaObservacion(string IdCabecera)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var selectItemsResumen = await _selectItems.ItemVistaNuevaObservacion();

                var tiposActoMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var responsables = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresMA = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorMAP)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresSeguridad = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorSSO)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresSeguridad.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.TiposActoMA = tiposActoMA;
                ViewBag.TiposCondicionMA = tiposCondicionMA;
                ViewBag.TiposActoSEG = tiposActoSEG;
                ViewBag.TiposCondicionSEG = tiposCondicionSEG;

                ViewBag.Responsables = responsables;
                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdCabecera));
                ViewBag.CabeceraEncriptada = IdCabecera;
                ViewBag.idCabecera = IdDesencriptado;

                ViewBag.AsesoresMA = asesoresMA;
                ViewBag.AsesoresSeguridad = asesoresSeguridad;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevaObservacion(NuevaObservacionModel model)
        {
            if (ModelState.IsValid)
            {
                var evidencias = new List<string>();
                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {                       
                        var newfileName = CreateUniqueFileName(i + 1)+".zip";
                        var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                        evidencias.Add(filePath);
                    }
                }
                var idObservacion = await _registrarInspeccionObservacion.Execute(
                            model.idCabecera,
                            model.TipoInspeccion,
                            model.Asesor_MA_Seguridad,
                            model.CondicionIdentificada,
                            model.TipoCondicion,
                            evidencias,
                            model.NiveRiesgo,
                            model.AccionTomada,
                            model.ResponsableMarc,
                            model.ResponsableCont,
                            model.FechaProgramada,
                            User.Identity.Name);
                var IdEncriptado = _protector.Protect(model.idCabecera.ToString());
                return RedirectToActionPermanent("EditarInspeccion", new { IdCabecera = IdEncriptado });
            }
            else
            {
                var IdEncriptado = _protector.Protect(model.idCabecera.ToString());
                return RedirectToAction("NuevaObservacion", new { IdCabecera = IdEncriptado });

            }
        }
        public async Task<IActionResult> EditarInspeccion(string IdCabecera)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var selectItemsResumen = await _selectItems.ItemVistaEditarInspeccion();

                var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                gerenciasEjecutorasMC.RemoveAt(1);

                var empresasMC = selectItemsResumen.EmpContratMarcobre.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresasMC.Insert(0, new SelectListItem("No Aplica", "1"));
                empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var responsables = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposInspeccion = new List<SelectListItem>();
                tiposInspeccion.Insert(0, new SelectListItem("OTRO", "OTRO", false));
                tiposInspeccion.Insert(0, new SelectListItem("NO PLANEADA", "NO PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("PLANEADA", "PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("--SELECCIONE--", "", true));
                var model = new NuevaInspeccionModel();
                if (IdCabecera.Length != 0)
                {
                    int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdCabecera));
                    var filaObservacion = "";
                    var observaciones = await _consultarInspeccionDetalle.Execute(IdDesencriptado,User.Identity.Name);
                    ViewBag.nObservacioness = observaciones.Count;

                    foreach (var observacion in observaciones)
                    {
                        var evidencia = "";
                        for (int i = 0; i < observacion.EvidenciaCondicion.Count; i++)
                        {
                            if (i == 0)
                                evidencia = evidencia + "<a href=\"/Inspecciones/Download?fileName=" + observacion.EvidenciaCondicion[i] + "\">Evidencia</a>";
                            else
                                evidencia = evidencia + "<a href=\"/Inspecciones/Download?fileName=" + observacion.EvidenciaCondicion[i] + "\">, Evidencia</a>";
                        }
                        filaObservacion += "<tr>";
                        filaObservacion += "<td style=\"display: none; \">" + observacion.IdInspeccionDetalle + "</td>";
                        filaObservacion += "<td>" + observacion.TipoInspeccion + "</td>";
                        filaObservacion += "<td>" + observacion.AsesorMASeguridad + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionCondicion + "</td>";
                        filaObservacion += "<td>" + observacion.TipoCondicion + "</td>";
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
                        filaObservacion += "<td>" + observacion.DescripcionAccion + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableMarcobre + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableContratista + "</td>";
                        filaObservacion += "<td>" + observacion.FechaProgramada.ToShortDateString() + "</td>";
                        if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) && observacion.Permisos)
                        {
                            filaObservacion += "<td><a class=\"btn btn-primary\" style=\"color:white\" onclick=\"ampliarInspeccion(" + observacion.IdInspeccionDetalle + ")\">Ampliar Fecha</a></td>";
                        }
                        filaObservacion += "</tr>";

                    }
                    ViewBag.ContenidoTabla = filaObservacion;
                


                ViewBag.GerenciaMC = gerenciasEjecutorasMC;
                ViewBag.EmpresasMC = empresasMC;
                ViewBag.Responsables = responsables;
                ViewBag.Tipo_Inspeccion = tiposInspeccion;

                var cabecera = await _consultarCabecera.Execute(IdDesencriptado, User.Identity.Name);

                if (cabecera != null)
                {
                    model.Id_Inspeccion_Cabecera = IdDesencriptado;
                    model.EncryptedId = IdCabecera;
                    model.Codigo = cabecera.Codigo;
                    model.Responsable_Inspeccion = cabecera.ResponsableInspeccion;
                    model.Tipo_Inspeccion = cabecera.TipoInspeccion;
                    model.Id_Empresa_Contratista = cabecera.IdEmpresa;
                    model.Id_Gerencia_Ejecutora = cabecera.IdGerencia;
                    model.Lugar_Inspeccionado = cabecera.AreaInspeccionada;
                    model.Id_Responsable_Lugar = cabecera.IdResponsableArea;
                    model.Fecha_Inspeccion = cabecera.FechaInspeccion.Year + "-" + (cabecera.FechaInspeccion.Month.ToString().Length > 1 ? cabecera.FechaInspeccion.Month.ToString() : "0" + cabecera.FechaInspeccion.Month.ToString()) + "-" + (cabecera.FechaInspeccion.Day.ToString().Length > 1 ? cabecera.FechaInspeccion.Day.ToString() : "0" + cabecera.FechaInspeccion.Day.ToString());
                    model.Hora_Inspeccion = (cabecera.FechaInspeccion.Hour.ToString().Length > 1 ? cabecera.FechaInspeccion.Hour.ToString() : "0" + cabecera.FechaInspeccion.Hour.ToString()) + ":" + (cabecera.FechaInspeccion.Minute.ToString().Length > 1 ? cabecera.FechaInspeccion.Minute.ToString() : "0" + cabecera.FechaInspeccion.Minute.ToString());
                    model.Objetivo = cabecera.Objetivo;
                    model.Inspectores = cabecera.Participantes;
                    model.Estado = cabecera.Estado;
                }

                ViewBag.Permiso = cabecera.Permiso;
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarInspeccion(NuevaInspeccionModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                {
                    await _editarInspeccionCabecera.Execute(
                    model.Id_Inspeccion_Cabecera,
                    model.Objetivo,
                    model.Id_Gerencia_Ejecutora,
                    model.Id_Empresa_Contratista,
                    model.Lugar_Inspeccionado,
                    model.Fecha_Inspeccion,
                    model.Hora_Inspeccion,
                    model.Id_Responsable_Lugar,
                    model.Inspectores,
                    model.opcion,
                    User.Identity.Name);
                    if (model.opcion.Equals("Observacion"))
                        return RedirectToAction("NuevaObservacion", new { IdCabecera = model.EncryptedId });
                    else if (model.opcion.Equals("Guardar"))
                        return RedirectToAction("NotificarInspección","Notificaciones", new { IdCabecera = model.Id_Inspeccion_Cabecera });
                }                
            }
            return RedirectToAction("Resumen");
        }
        [HttpPost]
        public async Task<IActionResult> AmpliarFechaObservacion(string idCabecera, int idObservacion, string fecha)
        {
            await _ampliarFechaObservacion.Execute(idObservacion, fecha,User.Identity.Name);
            return RedirectToAction("EditarInspeccion", new { IdCabecera = idCabecera });
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia__Registro_Observacion_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return fileName;
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }       
        public async Task<IActionResult> Download(string fileName)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + User.Identity.Name.Split("\\")[1] + "\\";
                    var archivePath = SGATempPath + "zip\\" + User.Identity.Name.Split("\\")[1] + "\\";

                    /*decrypt file*/
                    var decryptedFileName = "desencriptado.zip";
                    await DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
                    /*read decrypted file*/
                    FileStream stream = new FileStream(archivePath + decryptedFileName, FileMode.Open);

                    return File(stream, GetContentType(decryptedFileName), fileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }
        public async Task<string> UploadFTPFileAsync(string path, string newFileName)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0)
            {
                return "No Autorizado";
            }
            else
            {
                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaR);
                string route ="", user ="", pass = "";
                foreach(var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "route": route = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                    }
                }
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(route + newFileName);

                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                using (FileStream fs = System.IO.File.OpenRead(path))
                {

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    Stream requestStream = ftpRequest.GetRequestStream();
                    await requestStream.WriteAsync(buffer, 0, buffer.Length);
                    await requestStream.FlushAsync();
                    requestStream.Close();
                }
                return newFileName;
            }
        }
        public async Task<string> UploadZipFileAsync(IFormFile file,string newFileName)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario)==0)
            {
                return "No Autorizado";
            }
            else
            {
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + User.Identity.Name.Split("\\")[1] + "/";
                var archiveFile = SGATempPath + "zip/" + User.Identity.Name.Split("\\")[1] + "/archive.zip";
                var archivePath = SGATempPath + "zip/" + User.Identity.Name.Split("\\")[1] + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);

                var fileName = filePath + file.FileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    //CopyToAsync corrompe los archivos
                    file.CopyTo(stream);
                    stream.Close();
                }
                ZipFile.CreateFromDirectory(filePath, archiveFile);

                var encryptedArchiveFile = SGATempPath + "zip/" + User.Identity.Name.Split("\\")[1] + "/encriptado.zip";
                await EncryptFileAsync(archiveFile, archivePath + "encriptado.zip");
                //await DecryptFileAsync(archivePath + "encriptado.zip", archivePath + "desencriptado.zip");

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName);
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName)
        {
            try
            {
                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaR);
                string route = "", user = "", pass = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "route": route = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                    }
                }
                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(route + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                var ftpStream = ftpResponse.GetResponseStream();

                /* Save file in application server to decrypt */
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + User.Identity.Name.Split("\\")[1] + "/";
                var archivePath = SGATempPath + "zip/" + User.Identity.Name.Split("\\")[1] + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);
                /*New file downloaded name*/
                var downloadedFilePath = archivePath + fileName;
                /*Copy data to new temp file*/
                var fileStream =System.IO.File.Create(downloadedFilePath);
                ftpStream.CopyTo(fileStream);
                fileStream.Close();

                return downloadedFilePath;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        private async Task EncryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach(var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, IV),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        private async Task DecryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }
            RijndaelManaged RMCrypto = new RijndaelManaged();
            
            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, IV),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }
    }
}