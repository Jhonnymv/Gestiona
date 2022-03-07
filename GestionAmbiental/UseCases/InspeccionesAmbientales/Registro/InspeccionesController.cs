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
using GestionAmbiental.Application.UseCases.Inspecciones.EliminarVencidos;
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
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IEditarInspeccionCabecera _editarInspeccionCabecera;
        private readonly IEditarObservacion _editarObservacion;
        private readonly IAmpliarFechaObservacion _ampliarFechaObservacion;
        private readonly IConsultarObservacion _consultarObservacion;
        private readonly IEliminarObservacion _eliminarObservacion;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IEliminarVencidos _eliminarVencidos;

        public InspeccionesController(
            IConsultarCabeceras consultarCabeceras,
            ISelectItems selectItems,
            IRegistrarInspeccionCabecera registrarInspeccionCabecera,
            IWebHostEnvironment hostingEnvironment,
            IRegistrarInspeccionObservacion registrarInspeccionObservacion,
            IConsultarInspeccionDetalle consultarInspeccionDetalle,
            IConsultarCabecera consultarCabecera,
            IEditarInspeccionCabecera editarInspeccionCabecera,
            IEditarObservacion editarObservacion,
            IEliminarObservacion eliminarObservacion,
            IAmpliarFechaObservacion ampliarFechaObservacion,
            IConsultarObservacion consultarObservacion,
            IEliminarVencidos eliminarVencidos,
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
            _eliminarVencidos = eliminarVencidos;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
            _info_SGA = info_SGA;
        }
        public IActionResult PropiedadesModulo()
        {
            dynamic response;
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Inspection"));
                if (dataModulo != null)
                    response = new
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(dataModulo)
                    };
                else
                    response = new
                    {
                        success = false,
                        msg = "No se encontraron las propiedades del módulo"
                    };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    msg = e.Message
                };
            }
            return Ok(response);
        }
        public async Task<IActionResult> Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));            
            
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var selectItemsResumen = await _selectItems.ItemVistaResumen();

                var filtroEmpresas = selectItemsResumen.EmpContratMarcobre.Where(x=>x.Estado.Equals("ACT")).OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                var filtroGerencias = selectItemsResumen.GerenciaEjecutora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                var filtroUsuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres+ " "+g.Apellidos }).ToList();

                var usuariosAgrupados = selectItemsResumen.Usuarios.GroupBy(x => x.IdRol).ToList();
                var asesores = new List<SelectListItem>();
                foreach (var grupo in usuariosAgrupados)
                {
                    if(grupo.Key.Equals((int)InspeccionRol.AsesorMAP))
                    {
                        var nuevoGrupo = new SelectListGroup() { Name = "Asesores MAP", Disabled = false };
                        asesores.AddRange(grupo.OrderBy(x => x.Nombres).Select(i => new SelectListItem() { Value = i.IdUsuario.ToString(), Text = i.Nombres + " "+i.Apellidos, Group = nuevoGrupo }).ToList());
                    }
                    if(grupo.Key.Equals((int)InspeccionRol.AsesorSSO))
                    {
                        var nuevoGrupo = new SelectListGroup() { Name = "Asesores SSO", Disabled = false };
                        asesores.AddRange(grupo.OrderBy(x => x.Nombres).Select(i => new SelectListItem() { Value = i.IdUsuario.ToString(), Text = i.Nombres + " "+i.Apellidos, Group = nuevoGrupo }).ToList());
                    }                    
                }

                ViewBag.Empresas = filtroEmpresas;
                ViewBag.Gerencias = filtroGerencias;
                ViewBag.Usuarios = filtroUsuarios;
                ViewBag.Asesores = asesores;
                ViewBag.PermisoAdmin = userInfo.Rol.Equals(InspeccionRol.Admin);

                return View();
           }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string fechaInicioFiltro,string fechaFinFiltro, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, string[] estadoFiltro, int[] responsableFiltro, int[] asesorFiltro,string codigoFiltro, int pagina, int rpp)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            DateTime fechaInicio;
            DateTime fechaFin;
            if (fechaInicioFiltro != null)
                fechaInicio = Convert.ToDateTime(fechaInicioFiltro);
            else
                fechaInicio = (DateTime)SqlDateTime.MinValue;
            if (fechaFinFiltro != null)
                fechaFin = Convert.ToDateTime(fechaFinFiltro);
            else
                fechaFin = DateTime.Now;

            var datosTabla = await _consultarCabeceras.Execute(pagina, rpp, fechaInicio, fechaFin, gerenciaFiltro, empresaFiltro, registradoPorFiltro,estadoFiltro, responsableFiltro, asesorFiltro, codigoFiltro);
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
                    if(fila.Estado.Equals(EstadosInspeccion.Pendiente.ToString()) || userInfo.Rol.Equals(InspeccionRol.Admin))
                    filasTabla += "<td><a href=\"/Inspecciones/EditarInspeccion?IdCabecera=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    else
                        filasTabla += "<td><a href=\"/Inspecciones/DetalleInspeccion?IdCabecera=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    if(userInfo.Rol.Equals(InspeccionRol.Admin))
                        filasTabla += "<td>" + fila.RegistradoPor + "</td>";
                    filasTabla += "<td>" + fila.Gerencia + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.GerenciaInspeccionada + "</td>";
                    filasTabla += "<td>" + fila.AreaInspeccionada + "</td>";
                    filasTabla += "<td>" + fila.FechaInspeccion.ToString("dd/MM/yyyy HH:mm") + "</td>";
                    filasTabla += "<td>" + fila.NObservaciones + "</td>";
                    filasTabla += "<td style=\"width: 10%\"><div class=\"progress\">" +
                                          "<span class=\"percent\" >" + avance + "%</span>" +
                                          "<div class=\"progress-bar\" role=\"progressbar\" style=\"width:" + avance + "%; \" aria-valuemin=\"0\" aria-valuemax=\"100\">" 
                                           +
                                          "</div>" +
                                      "</div>" +
                                  "</td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarVencidos(string fechaInicioFiltro, string fechaFinFiltro)
        {
            dynamic response;
            try
            {
                await _eliminarVencidos.Execute(fechaInicioFiltro, fechaFinFiltro);
                response = new
                {
                    success = true,
                    message = "Exito"
                };
            }
            catch(Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok();
        }
        public async Task<IActionResult> NuevaInspeccion()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var selectItemsResumen = await _selectItems.ItemVistaEditarInspeccion();

                var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));                

                var empresasMC = selectItemsResumen.EmpContratMarcobre.Where(x=>x.Estado.Equals("ACT")).OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var responsables = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                var usuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresMA = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorMAP)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresSeguridad = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorSSO)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresSeguridad.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposInspeccion = new List<SelectListItem>();
                tiposInspeccion.Insert(0, new SelectListItem("OTRO", "OTRO", false));
                tiposInspeccion.Insert(0, new SelectListItem("NO PLANEADA", "NO PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("PLANEADA", "PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                ViewBag.GerenciaMC = gerenciasEjecutorasMC;
                ViewBag.EmpresasMC = empresasMC;
                ViewBag.Responsables = responsables;
                ViewBag.Usuarios = usuarios;
                ViewBag.Tipo_Inspeccion = tiposInspeccion;
                ViewBag.AsesoresMA = asesoresMA;
                ViewBag.AsesoresSeguridad = asesoresSeguridad;

                ViewBag.TiposActoMA = tiposActoMA;
                ViewBag.TiposCondicionMA = tiposCondicionMA;
                ViewBag.TiposActoSEG = tiposActoSEG;
                ViewBag.TiposCondicionSEG = tiposCondicionSEG;

                return View(new NuevaInspeccionModel());
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevaInspeccion(NuevaInspeccionModel model)
        {
            dynamic response;
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("SessionUser") != null)
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                    int idCabecera = await _registrarInspeccionCabecera.Execute(
                        model.Objetivo,
                        userInfo.NombreUsuario,
                        model.Id_Gerencia_Ejecutora,
                        model.Id_Empresa_Contratista,
                        model.Id_Gerencia_Inspeccionada,
                        model.Lugar_Inspeccionado,
                        model.Fecha_Inspeccion,
                        model.Hora_Inspeccion,
                        model.Tipo_Inspeccion,
                        model.Id_Responsable_Lugar,
                        model.Responsable_Inspeccion,
                        model.Inspectores,
                        model.opcion);
                    response = new
                    {
                        success = true,
                        idEncriptado = _protector.Protect(idCabecera.ToString()),
                        idCabecera = idCabecera
                    };
                    //if (model.opcion.Equals("Observacion")||model.opcion.Equals("GuardarEstado"))
                    //{
                    //var IdEncriptado = _protector.Protect(idCabecera.ToString());
                    //return RedirectToAction("EditarInspeccion", new { IdCabecera = IdEncriptado });
                    //}
                }
                else
                {
                    response = new
                    {
                        success = false,
                        message = "Usuario no inició sesión"
                    };
                }
            }
            else
            {
                response = new
                {
                    success = false,
                    message = "Datos incompletos"
                };
            }
            return Ok(response);
        }
        public async Task<IActionResult> NuevaObservacion(string IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdCabecera=" + IdCabecera });
            }
            else
            {
                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdCabecera));
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
            var IdEncriptado = _protector.Protect(model.idCabecera.ToString());
            dynamic response;
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
                            model.DescripcionActoCondicion,
                            model.CondicionIdentificada,
                            model.TipoCondicion,
                            evidencias,
                            model.NiveRiesgo,
                            model.AccionTomada,
                            model.ResponsableMarc,
                            model.ResponsableMarc2,
                            model.ResponsableCont,
                            model.CorreoCont,
                            model.FechaProgramada,
                            userInfo.NombreUsuario);                                
                response = new
                {
                    success = true,
                };
                return Ok(response);
                //return RedirectToActionPermanent("EditarInspeccion", new { IdCabecera = IdEncriptado });
            }
            else
            {
                response = new
                {
                    success = false,
                    message = "La información enviada no es válida"
                };
                return Ok(response);
                //return RedirectToAction("NuevaObservacion", new { IdCabecera = IdEncriptado });
            }
        }
        public async Task<IActionResult> ConsultarObservaciones(string IdCabecera)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdCabecera));
            dynamic response;
            try
            {
                var cabecera = await _consultarCabecera.Execute(IdDesencriptado, userInfo.NombreUsuario);
                var observaciones = await _consultarInspeccionDetalle.ExecuteRegistro(IdDesencriptado, userInfo.NombreUsuario);
                var filaObservacion = "";
                if (observaciones.Count > 0)
                {
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
                        if ((cabecera.Estado.Equals(EstadosInspeccion.Pendiente.ToString()) && cabecera.Permiso) || ((observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) && observacion.PermisosAsesor) || observacion.PermisosAdmin))
                            filaObservacion += "<td><button class=\"btn btn-mc\" type=\"button\" onclick=\"EditarObservacion('" + observacion.IdInspeccionDetalle + "')\">" + "Editar" + "</button></td>";
                        else
                            filaObservacion += "<td></td>";
                        if ((cabecera.Estado.Equals(EstadosInspeccion.Pendiente.ToString()) && cabecera.Permiso) || cabecera.PermisoAdmin)
                            filaObservacion += "<td><button class=\"btn btn-mc-danger\" type=\"button\" onclick=\"EliminarObservacion('" + IdObsEncriptado + "','" + observacion.IdInspeccionCabecera + "')\">" + "Eliminar" + "</button></td>";
                        //filaObservacion += "<td><a class=\"btn btn-danger\" href=\"/Inspecciones/EliminarObservacion?IdObservacion=" + IdObsEncriptado + "&IdCabecera=" + observacion.IdInspeccionCabecera + "\">" + "Eliminar" + "</a></td>";
                        else
                            filaObservacion += "<td></td>";

                        switch (observacion.Estado)
                        {
                            case "Aprobado": 
                                filaObservacion += "<td><span class=\"badge bg-success\">Aprobado</span></td>"; break;
                            case "Vencido":
                                filaObservacion += "<td><span class=\"badge bg-danger\">Vencido</span></td>"; break;
                            case "Implementado":
                                filaObservacion += "<td><span class=\"badge bg-info\">Pendiente de<br/>Verificación</span></td>"; break;
                            default:
                            //case "Rechazado":
                            //case "Ampliado": 
                            //case "Registrado":
                                filaObservacion += observacion.DescripcionCondicion.Equals("Buenas Prácticas")? ( observacion.Estado.Equals(EstadosInspeccion.Registrado.ToString())?"<td><span class=\"badge bg-info\">Pendiente de <br/>Verificación</span></td>":"<td><span class=\"badge bg-danger\">Rechazado</span></td>" ): "<td><span class=\"badge bg-warning\">Pendiente de<br/>Implementación</span></td>"; break;
                        }

                        filaObservacion += "<td>" + observacion.TipoInspeccion + "</td>";
                        filaObservacion += "<td>" + observacion.AsesorMASeguridad + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionCondicion + "</td>";
                        filaObservacion += "<td>" + observacion.TipoCondicion + "</td>";
                        filaObservacion += "<td>" + observacion.DescripcionActoCondicion + "</td>";
                        filaObservacion += "<td>" + evidencia + "</td>";
                        switch (observacion.NivelRiesgo)
                        {
                            case "Tolerable": filaObservacion += "<td><span class=\"badge bg-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alerta": filaObservacion += "<td><span class=\"badge bg-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "No Aceptable": filaObservacion += "<td><span class=\"badge bg-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Bajo": filaObservacion += "<td><span class=\"badge bg-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Medio": filaObservacion += "<td><span class=\"badge bg-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alto": filaObservacion += "<td><span class=\"badge bg-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            default: filaObservacion += "<td><span class=\"badge bg-secondary\">" + observacion.NivelRiesgo + "</span>"; break;
                        }
                        filaObservacion += "<td>" + observacion.DescripcionAccion + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableMarcobre + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableMarcobre2 + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableContratista + "</td>";
                        filaObservacion += "<td>" + observacion.CorreoContratista + "</td>";
                        filaObservacion += "<td>" + (observacion.FechaProgramada.HasValue?observacion.FechaProgramada.Value.ToShortDateString():"") + "</td>";                                             
                        filaObservacion += "</tr>";
                    }
                }
                else
                {
                    filaObservacion += "<tr>";
                    filaObservacion += "<td style=\"vertical-align:middle;\" colspan=\"100%\">No hay observaciones</td>";
                    filaObservacion += "</tr>";
                }
                var jsonObservaciones = JsonConvert.SerializeObject(observaciones.Select(x => new
                {
                    IdObservacion = x.IdInspeccionDetalle,
                    EvidenciasCondicion = JsonConvert.SerializeObject(x.EvidenciaCondicion),
                    TipoInspeccion = x.TipoInspeccion,
                    Asesor = x.IdAsesor,
                    DescripcionActoCondicion = x.DescripcionActoCondicion,
                    DescripcionCondicion = x.DescripcionCondicion,
                    TipoCondicion = x.IdTipoCondicion,
                    NivelRiesgo = x.NivelRiesgo,
                    Accion = x.DescripcionAccion,
                    ResponsableMarc = x.IdResponsableMarcobre,
                    ResponsableMarc2 = x.IdResponsableMarcobre2,
                    ResponsableCont = x.ResponsableContratista,
                    CorreoCont = x.CorreoContratista,
                    FechaProgramada = x.FechaProgramada.HasValue? x.FechaProgramada.Value.ToString("yyyy-MM-dd"):"",
                    Estado = x.Estado,
                    PermisoEditar = x.PermisosAsesor
                }));

                response = new
                {
                    success = true,
                    html = filaObservacion,
                    observaciones = jsonObservaciones
                };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
        }
        public async Task<IActionResult> EditarInspeccion(string IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdCabecera=" + IdCabecera });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));   
                
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var selectItemsResumen = await _selectItems.ItemVistaEditarInspeccion();

                var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresasMC = selectItemsResumen.EmpContratMarcobre.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresasMC.Insert(0, new SelectListItem("No Aplica", "1"));
                empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var responsables = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                var usuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                
                var tiposActoMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresMA = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorMAP)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresSeguridad = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorSSO)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresSeguridad.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                
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
                        model.Id_Gerencia_Inspeccionada = cabecera.IdGerenciaInspeccionada.HasValue? cabecera.IdGerenciaInspeccionada.Value:0;
                        model.Lugar_Inspeccionado = cabecera.AreaInspeccionada;
                        model.Id_Responsable_Lugar = cabecera.IdResponsableArea;
                        model.Fecha_Inspeccion = cabecera.FechaInspeccion.Year + "-" + (cabecera.FechaInspeccion.Month.ToString().Length > 1 ? cabecera.FechaInspeccion.Month.ToString() : "0" + cabecera.FechaInspeccion.Month.ToString()) + "-" + (cabecera.FechaInspeccion.Day.ToString().Length > 1 ? cabecera.FechaInspeccion.Day.ToString() : "0" + cabecera.FechaInspeccion.Day.ToString());
                        model.Hora_Inspeccion = (cabecera.FechaInspeccion.Hour.ToString().Length > 1 ? cabecera.FechaInspeccion.Hour.ToString() : "0" + cabecera.FechaInspeccion.Hour.ToString()) + ":" + (cabecera.FechaInspeccion.Minute.ToString().Length > 1 ? cabecera.FechaInspeccion.Minute.ToString() : "0" + cabecera.FechaInspeccion.Minute.ToString());
                        model.Objetivo = cabecera.Objetivo;
                        model.Inspectores = cabecera.Participantes;
                        model.Estado = cabecera.Estado;
                    }
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
                        if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()) && (observacion.PermisosAsesor || observacion.PermisosAdmin))
                        {
                            filaObservacion += "<td><a class=\"btn btn-mc\" style=\"color:white\" onclick=\"ampliarInspeccion(" + observacion.IdInspeccionDetalle + ")\">Ampliar Fecha</a></td>";
                        }
                        filaObservacion += "<td style=\"display: none; \">" + observacion.IdInspeccionDetalle + "</td>";
                        if (observacion.PermisosAsesor && cabecera.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                        {
                            filaObservacion += "<td><a class=\"btn btn-mc\" href=\"/Inspecciones/EditarObservacion?IdObservacion=" + IdObsEncriptado + "\">" + "Editar" + "</a></td>";
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
                        filaObservacion += "<td>" + observacion.DescripcionActoCondicion + "</td>";
                        filaObservacion += "<td>" + observacion.TipoCondicion + "</td>";
                        filaObservacion += "<td>" + evidencia + "</td>";
                        switch (observacion.NivelRiesgo)
                        {
                            case "Tolerable": filaObservacion += "<td><span class=\"badge bg-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alerta": filaObservacion += "<td><span class=\"badge bg-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "No Aceptable": filaObservacion += "<td><span class=\"badge bg-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Bajo": filaObservacion += "<td><span class=\"badge bg-success\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Medio": filaObservacion += "<td><span class=\"badge bg-warning\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            case "Alto": filaObservacion += "<td><span class=\"badge bg-danger\">" + observacion.NivelRiesgo + "</span></td>"; break;
                            default: filaObservacion += "<td><span class=\"badge bg-secondary\">" + observacion.NivelRiesgo + "</span>"; break;
                        }
                        filaObservacion += "<td>" + observacion.DescripcionAccion + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableMarcobre + "</td>";
                        filaObservacion += "<td>" + observacion.ResponsableContratista + "</td>";
                        filaObservacion += "<td>" + observacion.CorreoContratista + "</td>";
                        filaObservacion += "<td>" + (observacion.FechaProgramada.HasValue? observacion.FechaProgramada.Value.ToShortDateString():"") + "</td>";                        
                        filaObservacion += "</tr>";
                    }
                    ViewBag.ContenidoTabla = filaObservacion;      
                    ViewBag.GerenciaMC = gerenciasEjecutorasMC;
                    ViewBag.EmpresasMC = empresasMC;
                    ViewBag.Responsables = responsables;
                    ViewBag.Usuarios = usuarios;
                    ViewBag.Tipo_Inspeccion = tiposInspeccion;
                    ViewBag.nObservacioness = observaciones.Count;
                    ViewBag.Permiso = cabecera.Permiso;
                    ViewBag.PermisosAdmin = userInfo.Rol.Equals(InspeccionRol.Admin);

                    ViewBag.TiposActoMA = tiposActoMA;
                    ViewBag.TiposCondicionMA = tiposCondicionMA;
                    ViewBag.TiposActoSEG = tiposActoSEG;
                    ViewBag.TiposCondicionSEG = tiposCondicionSEG;

                    ViewBag.CabeceraEncriptada = IdCabecera;
                    ViewBag.idCabecera = IdDesencriptado;

                    ViewBag.AsesoresMA = asesoresMA;
                    ViewBag.AsesoresSeguridad = asesoresSeguridad;

                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarInspeccionNoSubmit(NuevaInspeccionModel model)
        {
            dynamic response;
            try
            {
                if (ModelState.IsValid)
                {
                    var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                    await _editarInspeccionCabecera.Execute(
                    model.Id_Inspeccion_Cabecera,
                    model.Objetivo,
                    model.Id_Gerencia_Ejecutora,
                    model.Id_Empresa_Contratista,
                    model.Id_Gerencia_Inspeccionada,
                    model.Lugar_Inspeccionado,
                    model.Tipo_Inspeccion,
                    model.Id_Responsable_Lugar,
                    model.Hora_Inspeccion,
                    model.Fecha_Inspeccion,
                    model.Responsable_Inspeccion,
                    model.Inspectores,
                    model.opcion,
                    userInfo.NombreUsuario);
                    response = new
                    {
                        success = true
                    };
                }
                else
                {
                    response = new
                    {
                        success = false,
                        message = "Datos incompletos"
                    };
                }
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    message = e.Message
                };
            }
            return Ok(response);
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
                model.Id_Gerencia_Inspeccionada,
                model.Lugar_Inspeccionado,
                model.Tipo_Inspeccion,
                model.Id_Responsable_Lugar,
                model.Hora_Inspeccion,
                model.Fecha_Inspeccion,
                model.Responsable_Inspeccion,
                model.Inspectores,
                model.opcion,
                userInfo.NombreUsuario);
                if (model.opcion.Equals("Observacion"))
                    return RedirectToAction("NuevaObservacion", new { IdCabecera = model.EncryptedId });
                else if (model.opcion.Equals("Guardar"))
                    return RedirectToAction("NotificarInspeccion", "NotificacionesInspecciones", new { IdCabecera = model.Id_Inspeccion_Cabecera });
                else
                    return RedirectToAction("Resumen");
            }
            return RedirectToAction("Resumen");
        }
        [HttpPost]
        public async Task<IActionResult> EditarObservacion(EditarObservacionModel model)
        {
            dynamic response;
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var evidencias = new List<string>();
                if (model.E_Nueva_Evidencia != null)
                {
                    for (int i = 0; i < model.E_Nueva_Evidencia.Count; i++)
                    {
                        var extension = Path.GetExtension(model.E_Nueva_Evidencia[i].FileName);
                        var max_file_size = 8388608;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.E_Nueva_Evidencia[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                            var filePath = await UploadZipFileAsync(model.E_Nueva_Evidencia[i], newfileName);
                            evidencias.Add(filePath);
                        }
                    }
                }
                await _editarObservacion.Execute(
                    model.E_IdCabecera,
                    model.E_IdObservacion,
                    model.E_TipoInspeccion,
                    model.E_AsesorSeguridad,
                    model.E_DescripcionActoCondicion,
                    model.E_CondicionIdentificada,
                    model.E_TipoCondicion,
                    evidencias.Count>0?evidencias:model.E_Evidencia,
                    model.E_NivelRiesgo,
                    model.E_AccionTomada,
                    model.E_ResponsableMarc,
                    model.E_ResponsableMarc2,
                    model.E_ResponsableCont,
                    model.E_CorreoCont,
                    model.E_FechaProgramada,
                    userInfo.NombreUsuario,
                    model.E_CambioFechaVencida);
                response = new
                {
                    success = true
                };
            }
            else
            {
                response = new
                {
                    success = false
                };
            }
            return Ok(response);
        }
        public IActionResult DetalleInspeccionNotificacion(int IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdCabecera=" + IdCabecera });
            }
            else
            {
                return RedirectToAction("DetalleInspeccion", new { IdCabecera = _protector.Protect(IdCabecera.ToString()) });
            }
        }
        public async Task<IActionResult> DetalleInspeccion(string IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdCabecera="+IdCabecera });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var selectItemsResumen = await _selectItems.ItemVistaEditarInspeccion();

                var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresasMC = selectItemsResumen.EmpContratMarcobre.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresasMC.Insert(0, new SelectListItem("No Aplica", "1"));
                empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var responsables = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                var usuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposActoSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposActoSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposCondicionSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                tiposCondicionSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresMA = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorMAP)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var asesoresSeguridad = selectItemsResumen.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorSSO)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresSeguridad.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposInspeccion = new List<SelectListItem>();
                tiposInspeccion.Insert(0, new SelectListItem("OTRO", "OTRO", false));
                tiposInspeccion.Insert(0, new SelectListItem("NO PLANEADA", "NO PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("PLANEADA", "PLANEADA", false));
                tiposInspeccion.Insert(0, new SelectListItem("--SELECCIONE--", "", true));
                var model = new NuevaInspeccionModel();
                if (IdCabecera.Length != 0)
                {
                    int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdCabecera));
                    var observaciones = await _consultarInspeccionDetalle.ExecuteRegistro(IdDesencriptado, userInfo.NombreUsuario);
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
                        model.Id_Gerencia_Inspeccionada = cabecera.IdGerenciaInspeccionada.HasValue?cabecera.IdGerenciaInspeccionada.Value:0;
                        model.Lugar_Inspeccionado = cabecera.AreaInspeccionada;
                        model.Id_Responsable_Lugar = cabecera.IdResponsableArea;
                        model.Fecha_Inspeccion = cabecera.FechaInspeccion.Year + "-" + (cabecera.FechaInspeccion.Month.ToString().Length > 1 ? cabecera.FechaInspeccion.Month.ToString() : "0" + cabecera.FechaInspeccion.Month.ToString()) + "-" + (cabecera.FechaInspeccion.Day.ToString().Length > 1 ? cabecera.FechaInspeccion.Day.ToString() : "0" + cabecera.FechaInspeccion.Day.ToString());
                        model.Hora_Inspeccion = (cabecera.FechaInspeccion.Hour.ToString().Length > 1 ? cabecera.FechaInspeccion.Hour.ToString() : "0" + cabecera.FechaInspeccion.Hour.ToString()) + ":" + (cabecera.FechaInspeccion.Minute.ToString().Length > 1 ? cabecera.FechaInspeccion.Minute.ToString() : "0" + cabecera.FechaInspeccion.Minute.ToString());
                        model.Objetivo = cabecera.Objetivo;
                        model.Inspectores = cabecera.Participantes;
                        model.Estado = cabecera.Estado;
                    }
                    ViewBag.GerenciaMC = gerenciasEjecutorasMC;
                    ViewBag.EmpresasMC = empresasMC;
                    ViewBag.Responsables = responsables;
                    ViewBag.Usuarios = usuarios;
                    ViewBag.Tipo_Inspeccion = tiposInspeccion;
                    ViewBag.nObservacioness = observaciones.Count;
                    ViewBag.Permiso = cabecera.Permiso;
                    ViewBag.PermisoAdmin = cabecera.PermisoAdmin;

                    ViewBag.TiposActoMA = tiposActoMA;
                    ViewBag.TiposCondicionMA = tiposCondicionMA;
                    ViewBag.TiposActoSEG = tiposActoSEG;
                    ViewBag.TiposCondicionSEG = tiposCondicionSEG;

                    ViewBag.CabeceraEncriptada = IdCabecera;
                    ViewBag.idCabecera = IdDesencriptado;

                    ViewBag.AsesoresMA = asesoresMA;
                    ViewBag.AsesoresSeguridad = asesoresSeguridad;

                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EliminarObservacion(string IdObservacion, string IdCabecera)
        {
            dynamic response;
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                //return RedirectToAction("IdObservacion", new { IdObservacion = IdObservacion });
                response = new
                {
                    success = false
                };
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
                await _eliminarObservacion.Execute(IdObsDesencriptado, userInfo.NombreUsuario);
                //return RedirectToAction("EditarInspeccion", new { IdCabecera = IdCabecera });
                response = new
                {
                    success = true
                };
            }
            return Ok(response);
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?fileName=" + fileName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            
                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre-v2\\";
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
                var SGATempPath = tempPath + "SGA-Marcobre-v2/";
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
                var SGATempPath = tempPath + "SGA-Marcobre-v2/";
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