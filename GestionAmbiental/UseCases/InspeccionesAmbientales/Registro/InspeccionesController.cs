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
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.EliminarObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.RegistrarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Registro
{
    public class InspeccionesController : Controller
    {

        private readonly IConsultarCabeceras _consultarCabeceras;
        private readonly IConsultarCabecera _consultarCabecera;
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarInspeccionCabecera _registrarInspeccionCabecera;
        private readonly IRegistrarInspeccionObservacion _registrarInspeccionObservacion;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IEditarInspeccionCabecera _editarInspeccionCabecera;
        private readonly IEditarObservacion _editarObservacion;
        private readonly IAmpliarFechaObservacion _ampliarFechaObservacion;
        private readonly IConsultarObservacion _consultarObservacion;
        private readonly IEliminarObservacion _eliminarObservacion;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;

        public InspeccionesController(
            IConsultarCabeceras consultarCabeceras,
            ISelectItems selectItems,
            IRegistrarInspeccionCabecera registrarInspeccionCabecera,
            IHostingEnvironment hostingEnvironment,
            IRegistrarInspeccionObservacion registrarInspeccionObservacion,
            IConsultarInspeccionDetalle consultarInspeccionDetalle,
            IConsultarCabecera consultarCabecera,
            IEditarInspeccionCabecera editarInspeccionCabecera,
            IEditarObservacion editarObservacion,
            IEliminarObservacion eliminarObservacion,
            IAmpliarFechaObservacion ampliarFechaObservacion,
            IConsultarObservacion consultarObservacion,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _consultarCabeceras = consultarCabeceras;
            _selectItems = selectItems;
            _registrarInspeccionCabecera = registrarInspeccionCabecera;
            _hostingEnvironment = hostingEnvironment;
            _registrarInspeccionObservacion = registrarInspeccionObservacion;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarCabecera = consultarCabecera;
            _editarInspeccionCabecera = editarInspeccionCabecera;
            _editarObservacion = editarObservacion;
            _eliminarObservacion = eliminarObservacion;
            _consultarObservacion = consultarObservacion;
            _ampliarFechaObservacion = ampliarFechaObservacion;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
            _info_SGA = info_SGA;
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

                var selectItemsResumen = await _selectItems.ItemVistaResumen();

                var filtroEmpresas = selectItemsResumen.EmpContratMarcobre.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                filtroEmpresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var filtroUsuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres+ " "+g.Apellidos }).ToList();
                filtroUsuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Empresas = filtroEmpresas;
                ViewBag.Usuarios = filtroUsuarios;

                return View();
           }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, int filtroEmpresa,int filtroUsuario, string filtroCodigo, int pagina, int rpp)
        {
            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarCabeceras.Execute(pagina, rpp, fecha, filtroEmpresa,filtroUsuario, filtroCodigo);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NCabeceras % rpp != 0 ? (datosTabla.NCabeceras / rpp) + 1 : datosTabla.NCabeceras / rpp;
            if (datosTabla.NCabeceras > 0)
            {
                foreach (var fila in datosTabla.Cabeceras)
                {
                    var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    var obsAvance = fila.NObsAprobadas;
                    double avance = obsAvance != 0 ? Math.Round((float)obsAvance / fila.NObservaciones * 100, 1) : 0;
                    filasTabla += "<td><a href=\"/Inspecciones/EditarInspeccion?IdCabecera=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.Gerencia + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.AreaInspeccionada + "</td>";
                    filasTabla += "<td>" + fila.FechaInspeccion.ToString("dd/MM/yyyy HH:mm") + "</td>";
                    filasTabla += "<td>" + fila.NObservaciones + "</td>";
                    filasTabla += "<td><div class=\"progress\"><div class=\"progress-bar bg-info\" role=\"progressbar\" style=\"width:" + avance + "%; color: black;font-weight: bold;\" aria-valuemin=\"0\" aria-valuemax=\"100\">" + avance + "%</div></div></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
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
                    var observaciones = await _consultarInspeccionDetalle.Execute(idCabecera, userInfo.NombreUsuario);
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
                if (HttpContext.Session.GetString("SessionUser") != null)
                {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                int idCabecera = await _registrarInspeccionCabecera.Execute(
                    model.Objetivo, userInfo.NombreUsuario, model.Id_Gerencia_Ejecutora, model.Id_Empresa_Contratista, model.Lugar_Inspeccionado, model.Fecha_Inspeccion, model.Hora_Inspeccion,
                    model.Tipo_Inspeccion, model.Id_Responsable_Lugar, model.Responsable_Inspeccion, model.Inspectores, model.opcion);
                    if (model.opcion.Equals("Observacion"))
                    {
                        var IdEncriptado = _protector.Protect(idCabecera.ToString());

                        return RedirectToAction("NuevaObservacion", new { IdCabecera = IdEncriptado });
                    }
                }
                else
                {
                    RedirectToAction("Signin", "Autenticacion");
                }
            }
            return RedirectToAction("Resumen");
        }
        public async Task<IActionResult> NuevaObservacion(string IdCabecera)
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
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var evidencias = new List<string>();
                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias[i].FileName);
                        var max_file_size = 8388608;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                            evidencias.Add(filePath);
                        }
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
                            userInfo.NombreUsuario);
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
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));   
                
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
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

                    var observaciones = await _consultarInspeccionDetalle.ExecuteRegistro(IdDesencriptado,userInfo.NombreUsuario);
                    ViewBag.nObservacioness = observaciones.Count;

                    var cabecera = await _consultarCabecera.Execute(IdDesencriptado, userInfo.NombreUsuario);

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
                    ViewBag.PermisoAdmin = cabecera.PermisoAdmin;

                    foreach (var observacion in observaciones)
                    {
                        var evidencia = "";
                        var IdObsEncriptado = _protector.Protect(observacion.IdInspeccionDetalle.ToString());
                        for (int i = 0; i < observacion.EvidenciaCondicion.Count; i++)
                        {
                            if (i == 0)
                                evidencia = evidencia + "<a href=\"/Inspecciones/Download?fileName=" + observacion.EvidenciaCondicion[i] + "\">Evidencia</a>";
                            else
                                evidencia = evidencia + "<a href=\"/Inspecciones/Download?fileName=" + observacion.EvidenciaCondicion[i] + "\">, Evidencia</a>";
                        }
                        filaObservacion += "<tr>";
                        filaObservacion += "<td style=\"display: none; \">" + observacion.IdInspeccionDetalle + "</td>";
                        if (observacion.Permisos && cabecera.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                        {
                            filaObservacion += "<td><a class=\"btn btn-primary\" href=\"/Inspecciones/EditarObservacion?IdObservacion=" + IdObsEncriptado + "\">" + "Editar" + "</a></td>";
                            filaObservacion += "<td><a class=\"btn btn-danger\" href=\"/Inspecciones/EliminarObservacion?IdObservacion=" + IdObsEncriptado+ "&IdCabecera="+IdCabecera+"\">" + "Eliminar" + "</a></td>";
                        }
                        else 
                        { 
                            filaObservacion += "<td></td>";
                            filaObservacion += "<td></td>";
                        }
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
                        if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) && (observacion.Permisos||observacion.PermisosAdmin))
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
                    
                    
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarInspeccion(NuevaInspeccionModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                    
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
                    userInfo.NombreUsuario);
                    if (model.opcion.Equals("Observacion"))
                        return RedirectToAction("NuevaObservacion", new { IdCabecera = model.EncryptedId });
                    else if (model.opcion.Equals("Guardar"))
                        return RedirectToAction("NotificarInspeccion","Notificaciones", new { IdCabecera = model.Id_Inspeccion_Cabecera });
                    else if(model.opcion.Equals("Eliminar"))
                        return RedirectToAction("Resumen");
            }
            return RedirectToAction("Resumen");
        }
        public async Task<IActionResult> EditarObservacion(string IdObservacion)
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

                int IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
                
                var observacion = await _consultarObservacion.Execute(IdObsDesencriptado);
                
                var model = new EditarObservacionModel();
                model.idCabecera = observacion.IdInspeccionCabecera;
                model.IdObservacion = observacion.IdInspeccionDetalle;
                model.IdObservacionEncryp = IdObservacion;
                model.TipoInspeccion = observacion.TipoInspeccion;
                model.Asesor_MA_Seguridad = observacion.IdAsesor;
                model.CondicionIdentificada = observacion.DescripcionCondicion;
                model.TipoCondicion = observacion.IdTipoCondicion;
                model.Evidencia = observacion.EvidenciaCondicion;
                model.NiveRiesgo = observacion.NivelRiesgo;
                model.AccionTomada = observacion.DescripcionAccion;
                model.ResponsableMarc = observacion.IdResponsableMarcobre;
                model.ResponsableCont = observacion.ResponsableContratista;
                model.FechaProgramada = observacion.FechaProgramada.ToString("yyyy-MM-dd");
                                            
                ViewBag.TiposActoMA = tiposActoMA;
                ViewBag.TiposCondicionMA = tiposCondicionMA;
                ViewBag.TiposActoSEG = tiposActoSEG;
                ViewBag.TiposCondicionSEG = tiposCondicionSEG;

                ViewBag.Responsables = responsables;
                
                ViewBag.CabeceraEncriptada = _protector.Protect(observacion.IdInspeccionCabecera.ToString());
                ViewBag.idCabecera = observacion.IdInspeccionCabecera;

                ViewBag.AsesoresMA = asesoresMA;
                ViewBag.AsesoresSeguridad = asesoresSeguridad;
            
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarObservacion(EditarObservacionModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var evidencias = new List<string>();
                if (model.Nueva_Evidencia != null)
                {
                    for (int i = 0; i < model.Nueva_Evidencia.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Nueva_Evidencia[i].FileName);
                        var max_file_size = 8388608;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Nueva_Evidencia[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                            var filePath = await UploadZipFileAsync(model.Nueva_Evidencia[i], newfileName);
                            evidencias.Add(filePath);
                        }
                    }
                }
                await _editarObservacion.Execute(
                    model.idCabecera,
                    model.IdObservacion,
                    model.TipoInspeccion,
                    model.Asesor_MA_Seguridad,
                    model.CondicionIdentificada,
                    model.TipoCondicion,
                    evidencias.Count>0?evidencias:model.Evidencia,
                    model.NiveRiesgo,
                    model.AccionTomada,
                    model.ResponsableMarc,
                    model.ResponsableCont,
                    model.FechaProgramada,
                    userInfo.NombreUsuario);
                var IdEncriptado = _protector.Protect(model.idCabecera.ToString());
                return RedirectToActionPermanent("EditarInspeccion", new { IdCabecera = IdEncriptado });
            }
            else
            {
                var IdEncriptado = _protector.Protect(model.idCabecera.ToString());
                return RedirectToAction("EditarInspeccion", new { IdCabecera = IdEncriptado });

            }
            return View();
        }        
        public async Task<IActionResult> EliminarObservacion(string IdObservacion,string IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("IdObservacion", new { IdObservacion = IdObservacion });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));            
                var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
                await _eliminarObservacion.Execute(IdObsDesencriptado, userInfo.NombreUsuario);
                return RedirectToAction("EditarInspeccion", new { IdCabecera = IdCabecera });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AmpliarFechaObservacion(string idCabecera, int idObservacion, string fecha)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            await _ampliarFechaObservacion.Execute(idObservacion, fecha,userInfo.NombreUsuario);
            return RedirectToAction("EditarInspeccion", new { IdCabecera = idCabecera });
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Ev_Reg_Obs_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
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
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            
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
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

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
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return "NoAutorizado";
            else 
            {           
                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaR);
                string host = "",port="", user = "", pass = "",dir="";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "host": host = item.Value; break;
                        case "port": port = item.Value;break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                        case "directory": dir = item.Value;break;
                    }
                }
                
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {                   
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);
                    using (FileStream fs = System.IO.File.OpenRead(path))
                    {
                        sftp.UploadFile(fs, newFileName, true);
                    }
                    sftp.Dispose();                    
                }            
                return newFileName;
            }
        }
        public async Task<string> UploadZipFileAsync(IFormFile file, string newFileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else 
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/archive.zip";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

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

                var encryptedArchiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/encriptado.zip";
                await EncryptFileAsync(archiveFile, archivePath + "encriptado.zip");                

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName);
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaR);
                string host = "", port = "", user = "", pass = "", dir = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "host": host = item.Value; break;
                        case "port": port = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                        case "directory": dir = item.Value; break;
                    }
                }

                /* Save file in application server to decrypt */
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

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
                var fileStream = System.IO.File.Create(downloadedFilePath);
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);
                    
                    //Descargar archivo de sftp a nuevo archivo temporal creado
                    sftp.DownloadFile(fileName, fileStream);

                    sftp.Dispose();
                }
                //ftpStream.CopyTo(fileStream);
                fileStream.Close();

                return downloadedFilePath;
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