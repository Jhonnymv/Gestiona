using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.AmpliarFechaAccion;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EditarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EliminarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.RegistrarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EditarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EliminarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.RegistrarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EditarReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EliminarEvidencias;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EliminarReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEstadoReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEvidenciasReporte;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportesPreliminares;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.EditarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.EliminarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.GetInvolucrados;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.RegistrarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte1;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte2;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.FOCAM;
using GestionAmbiental.UseCases.Incidentes.InvestigacionFinal;
using GestionAmbiental.UseCases.Util.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Incidentes.ReportePreliminar
{
    public class ReportePreliminarController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;

        private readonly IGetEvidenciasReporte _getEvidenciasReporte;
        private readonly IRegistrarParte1 _registrarReporteParte1;
        private readonly IRegistrarParte2 _registrarReporteParte2;
        private readonly IGetReportesPreliminares _getReportesPreliminares;
        private readonly IGetEstadoReportePreliminar _getEstadoReportePreliminar;
        private readonly ISelectItems _selectItems;
        private readonly IGetInvolucradosIncidente _getInvolucradosIncidente;
        private readonly IGetInvolucradoIncidente _getInvolucradoIncidente;
        private readonly IEditarInvolucradoIncidente _editarInvolucradoIncidente;
        private readonly IEliminarInvolucradoIncidente _eliminarInvolucradoIncidente;
        private readonly IRegistrarInvolucradoIncidente _registrarInvolucradoIncidente;
        private readonly IRegistrarAccionCorrectivaIncidente _registrarAccionCorrectivaIncidente;
        private readonly IEditarAccionIncidente _editarAccionIncidente;
        private readonly IGetAccioIncidente _getAccionIncidente;
        private readonly IEliminarAccionIncidente _eliminarAccionIncidente;
        private readonly IGetAccionesIncidente _getAccionesIncidente;
        private readonly IAmpliarFechaAccion _ampliarFechaAccion;
        private readonly IGetReportePreliminar _getReportePreliminar;
        private readonly IGetCausasIncidenteReporte _getCausasIncidenteReporte;
        private readonly IGetCausaIncidenteReporte _getCausaIncidenteReporte;
        private readonly IEditarCausaIncidenteReporte _editarCausaIncidenteReporte;
        private readonly IEliminarCausaIncidenteReporte _eliminarCausaIncidenteReporte;
        private readonly IRegistrarCausaIncidenteReporte _registrarCausaIncidenteReporte;
        private readonly IEditarReportePreliminar _editarReportePreliminar;
        private readonly IEliminarReportePreliminar _eliminarReportePreliminar;
        private readonly IEliminarEvidencia _eliminarEvidencia;
        private readonly IGetInfo_SGA _info_SGA;

        private readonly IDataProtector _protector;


        public ReportePreliminarController(
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IWebHostEnvironment hostingEnvironment,
            IGetEvidenciasReporte getEvidenciasReporte,
            IRegistrarParte1 registrarReporteParte1,
            IRegistrarParte2 registrarReporteParte2,
            IGetReportesPreliminares getReportesPreliminares,
            IGetEstadoReportePreliminar getEstadoReportePreliminar,
            ISelectItems selectItems,
            IGetInvolucradosIncidente getInvolucradosIncidente,
            IGetInvolucradoIncidente getInvolucradoIncidente,
            IEditarInvolucradoIncidente editarInvolucradoIncidente,
            IEliminarInvolucradoIncidente eliminarInvolucradoIncidente,
            IRegistrarInvolucradoIncidente registrarInvolucradoIncidente,
            IRegistrarAccionCorrectivaIncidente registrarAccionCorrectivaIncidente,
            IEditarAccionIncidente editarAccionIncidente,
            IGetAccioIncidente getAccioIncidente,
            IGetAccionesIncidente getAccionesIncidente,
            IEliminarAccionIncidente eliminarAccionIncidente,
            IAmpliarFechaAccion ampliarFechaAccion,
            IGetReportePreliminar getReportePreliminar,
            IGetCausasIncidenteReporte getCausasIncidenteReporte,
            IGetCausaIncidenteReporte getCausaIncidenteReporte,
            IEditarCausaIncidenteReporte editarCausaIncidenteReporte,
            IEliminarCausaIncidenteReporte eliminarCausaIncidenteReporte,
            IRegistrarCausaIncidenteReporte registrarCausaIncidenteReporte,
            IEditarReportePreliminar editarReportePreliminar,
            IEliminarReportePreliminar eliminarReportePreliminar,
            IEliminarEvidencia eliminarEvidencia,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _hostingEnvironment = hostingEnvironment;
            _getEvidenciasReporte = getEvidenciasReporte;
            _registrarReporteParte1 = registrarReporteParte1;
            _registrarReporteParte2 = registrarReporteParte2;
            _getReportesPreliminares = getReportesPreliminares;
            _getEstadoReportePreliminar = getEstadoReportePreliminar;
            _selectItems = selectItems;
            _getInvolucradosIncidente = getInvolucradosIncidente;
            _getInvolucradoIncidente = getInvolucradoIncidente;
            _registrarInvolucradoIncidente = registrarInvolucradoIncidente;
            _editarInvolucradoIncidente = editarInvolucradoIncidente;
            _eliminarInvolucradoIncidente = eliminarInvolucradoIncidente;
            _registrarAccionCorrectivaIncidente = registrarAccionCorrectivaIncidente;
            _editarAccionIncidente = editarAccionIncidente;
            _getAccionIncidente = getAccioIncidente;
            _getAccionesIncidente = getAccionesIncidente;
            _eliminarAccionIncidente = eliminarAccionIncidente;
            _ampliarFechaAccion = ampliarFechaAccion;
            _getReportePreliminar = getReportePreliminar;
            _getCausasIncidenteReporte = getCausasIncidenteReporte;
            _editarCausaIncidenteReporte = editarCausaIncidenteReporte;
            _eliminarCausaIncidenteReporte = eliminarCausaIncidenteReporte;
            _getCausaIncidenteReporte = getCausaIncidenteReporte;
            _registrarCausaIncidenteReporte = registrarCausaIncidenteReporte;
            _editarReportePreliminar = editarReportePreliminar;
            _eliminarReportePreliminar = eliminarReportePreliminar;
            _eliminarEvidencia = eliminarEvidencia;
            _info_SGA = info_SGA;

            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.ReportePreliminarIdRouteValue);
        }
        public IActionResult PropiedadesModulo()
        {
            dynamic response;
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Incidentes"));
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
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var items = await _selectItems.GetSelectItems();

                var asesores = items.ItemsUsuarioMA.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();

                var usuarios = items.ItemsUsuario.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).OrderBy(x => x.Text).ToList();

                var gerencias = items.ItemsGerencia.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();

                ViewBag.Asesores = asesores;
                ViewBag.Gerencias = gerencias;
                ViewBag.Usuarios = usuarios;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int rpp, int pagina)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var datosTabla = await _getReportesPreliminares.Execute(fecha_inicio, fecha_fin, gerencia, titular, estado, responsable, asesor, codigo, pagina, rpp, userInfo.NombreUsuario);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NReportes % rpp != 0 ? (datosTabla.NReportes / rpp) + 1 : datosTabla.NReportes / rpp;
            if (datosTabla.NReportes > 0)
            {
                foreach (var fila in datosTabla.Reportes)
                {
                    if (!fila.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) || (fila.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) && userInfo.Rol.Equals(InspeccionRol.Admin)))
                    {
                        var IdEncriptado = _protector.Protect(fila.Id_Reporte_Preliminar.ToString());
                        filasTabla += "<tr>";
                        if (fila.Estado.Equals(EstadosIncidentes.Pendiente.ToString()))
                            filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                        else
                            filasTabla += "<td></td>";
                        if (!fila.Estado.Equals(EstadosIncidentes.Eliminado.ToString()))
                            filasTabla += "<td style=\"width: 115px;\"><a href=\"/ReportePreliminar/EditarReportePreliminar?IdReporte=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                        else
                            filasTabla += "<td style=\"width: 115px;\"><a href=\"/ReportePreliminar/EditarReportePreliminar?IdReporte=" + IdEncriptado + "\">" + fila.Codigo + " (Eliminado)" + "</a></td>";
                        filasTabla += "<td>" + fila.Tipo_Incidente + "</td>";
                        filasTabla += "<td>" + fila.Nivel_Incidente + "</td>";
                        filasTabla += "<td style=\"width: 150px;\">";
                        filasTabla += fila.Fecha_Incidente != null ? fila.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm") : "";
                        filasTabla += "</td>";
                        filasTabla += "<td>" + fila.Producto_Derramado + "</td>";
                        filasTabla += "<td>" + fila.Cantidad_Derramado + " " + fila.Unidad_Cantidad_Derramado + "</td>";
                        filasTabla += "<td>" + fila.Area_Afectada + " " + fila.Unidad_Area_Afectada + "</td>";
                        filasTabla += "<td>" + fila.Lugar_Incidente + "</td>";
                        filasTabla += "<td>" + fila.Equipo_Involucrado + "</td>";
                        filasTabla += "<td>" + fila.Gerencia_Responsable + "</td>";
                        filasTabla += "</tr>";
                    }
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
                var nCol = 10;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"" + nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> NuevoReporte()
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
                var items = await _selectItems.GetSelectItems();

                var tipo_Incidente = items.ItemsTipoIncidente.Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
                tipo_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nivel_Incidente = items.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
                nivel_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Derramado = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                    .OrderBy(x => x.Descripcion)
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Area = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Area.ToString()))
                    .OrderBy(x => x.Descripcion)
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Area.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var componente_Ambiental = items.ItemsCompAmb.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Componente_Ambiental.ToString(), Text = g.Descripcion }).ToList();
                componente_Ambiental.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //Causa Incidente
                var grupoActo = new SelectListGroup() { Name = "Acto Subestándar", Disabled = false };
                var grupoCondicion = new SelectListGroup() { Name = "Condición Subestándar", Disabled = false };

                var causa_Incidente = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoActo }).ToList();
                causa_Incidente.AddRange(items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoCondicion }).ToList());
                causa_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Tipo_Incidente = tipo_Incidente;
                ViewBag.Nivel_Incidente = nivel_Incidente;
                ViewBag.Unidad_Derramado = unidad_Derramado;
                ViewBag.Unidad_Area = unidad_Area;
                ViewBag.Componente_Ambiental = componente_Ambiental;
                ViewBag.Causa_Incidente = causa_Incidente;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoReporte(ReporteParte1Model model)
        {
            if (ModelState.IsValid)
            {
                var IdReporte = await _registrarReporteParte1.Execute(
                    model.Num_Trabajadores,
                    model.Razon_Social_Contratista,
                    model.RUC_Contratista,
                    model.Actividad_Contratista,
                    model.Domicilio_Contratista,
                    model.Num_Trabajadores_Contratista,
                    model.Id_Tipo_Incidente,
                    model.Id_Nivel_Incidente,
                    model.Fecha_Incidente,
                    model.Hora_Incidente,
                    model.Producto_Derramado,
                    model.Cantidad_Derramado,
                    model.Id_Unidad_Cantidad_Derramado,
                    model.Area_Afectada,
                    model.Id_Unidad_Area_Afectada,
                    model.Lugar_Incidente,
                    model.Equipo_Involucrado,
                    model.Id_Componente_Ambiental,
                    model.Descripcion_Incidente);
                if (model.Opcion.Equals("Guardar"))
                    return RedirectToAction("Resumen");
                else
                {
                    var IdEncriptado = _protector.Protect(IdReporte.ToString());
                    return RedirectToAction("NuevoReporteParte2", new { IdReporte = IdEncriptado });
                }
            }
            return RedirectToAction("NuevoReporte");
        }
        public async Task<IActionResult> NuevoReporteParte2(string IdReporte)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdReporte=" + IdReporte });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var items = await _selectItems.GetSelectItems();

                var gerencias = items.ItemsGerencia.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var supervisores = items.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
                supervisores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
                usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var actos = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                actos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var condiciones = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Gerencias = gerencias;
                ViewBag.Supervisor = supervisores;
                ViewBag.Usuario = usuario;
                ViewBag.Actos = actos;
                ViewBag.Condiciones = condiciones;

                var model = new ReporteParte2Model();
                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdReporte));
                model.Id_Reporte_Preliminar = IdDesencriptado;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ListarInvolucrados(int IdReporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var involucrados = await _getInvolucradosIncidente.Execute(IdReporte);
            if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                involucrados = involucrados.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var estadoReporte = await _getEstadoReportePreliminar.Execute(IdReporte);
            var filas_involucrados = "";
            foreach (var fila in involucrados)
            {
                filas_involucrados += "<tr>";
                filas_involucrados += "<td>" + fila.Tipo_Involucrado + "</td>";
                filas_involucrados += "<td>" + fila.Nombres.ToUpper() + " " + fila.Apellidos.ToUpper() + "</td>";
                if (fila.Email != null)
                    filas_involucrados += "<td>" + fila.Email + "</td>";
                else
                    filas_involucrados += "<td>_</td>";
                if (fila.Celular != null)
                    filas_involucrados += "<td>" + fila.Celular + "</td>";
                else
                    filas_involucrados += "<td>_</td>";
                if (fila.Puesto_Trabajo != null)
                    filas_involucrados += "<td>" + fila.Puesto_Trabajo + "</td>";
                else
                    filas_involucrados += "<td>_</td>";
                if (fila.Turno != null)
                    filas_involucrados += "<td>" + fila.Turno + "</td>";
                else
                    filas_involucrados += "<td>_</td>";
                if (estadoReporte.Equals(EstadosIncidentes.Pendiente.ToString()) || userInfo.Rol.Equals(InspeccionRol.Admin))
                {
                    filas_involucrados += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarInvolucrado(" + fila.Id_Involucrado_Incidente + ")\">Editar</button></td>";
                    filas_involucrados += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarInvolucrado(" + fila.Id_Involucrado_Incidente + ")\">Eliminar</button></td>";
                }
                filas_involucrados += "</tr>";
            }
            dynamic response = new
            {
                filas_involucrados = filas_involucrados,
                nInvolucrados = involucrados.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevoInvolucrado(int IdReporte, string tipo, int usuario, string nombres, string apellidos, string correo, string puesto, string celular, string turno)
        {
            await _registrarInvolucradoIncidente.Execute(IdReporte, tipo, usuario, nombres, apellidos, correo, puesto, celular, turno);
            return await ListarInvolucrados(IdReporte);
        }
        [HttpGet]
        public async Task<IActionResult> EditarInvolucrado(int IdInvolucrado)
        {
            var involucrado = await _getInvolucradoIncidente.Execute(IdInvolucrado);
            dynamic response = new
            {
                idInvolucrado = involucrado.Id_Involucrado_Incidente,
                tipo = involucrado.Tipo_Involucrado,
                idUsuario = involucrado.Id_Usuario,
                nombres = involucrado.Nombres.ToUpper(),
                apellidos = involucrado.Apellidos.ToUpper(),
                email = involucrado.Email,
                celular = involucrado.Celular,
                puesto = involucrado.Puesto_Trabajo,
                turno = involucrado.Turno,

            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarInvolucrado(int IdInvolucrado, int IdReporte, string tipo, int usuario, string nombres, string apellidos, string correo, string puesto, string celular, string turno)
        {
            await _editarInvolucradoIncidente.Execute(IdInvolucrado, IdReporte, tipo, usuario, nombres, apellidos, correo, puesto, celular, turno);
            return await ListarInvolucrados(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarInvolucrado(int IdInvolucrado, int IdReporte)
        {
            await _eliminarInvolucradoIncidente.Execute(IdInvolucrado);
            return await ListarInvolucrados(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> ListarAccionesVerificacion(int IdReporte)
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

                var acciones = await _getAccionesIncidente.Execute(IdReporte, userInfo.NombreUsuario);

                var nVerificado = acciones.Count(x => x.Estado.Equals(EstadosIncidentes.Aprobado.ToString()));

                var estadoReporte = await _getEstadoReportePreliminar.Execute(IdReporte);

                var filas_acciones = "";
                if (acciones.Count > 0)
                {
                    foreach (var fila in acciones)
                    {
                        var IdEncriptado = _protector.Protect(fila.Id_Accion_Correctiva_Incidente.ToString());

                        filas_acciones += "<tr>";
                        if (fila.Accion_Inmediata != null)
                            filas_acciones += "<td>" + fila.Accion_Inmediata + "</td>";
                        else
                            filas_acciones += "<td>_</td>";
                        if (fila.Responsable_Marcobre != null)
                            filas_acciones += "<td>" + fila.Responsable_Marcobre + "</td>";
                        else
                            filas_acciones += "<td>_</td>";
                        if (fila.Responsable_Marcobre2 != null)
                            filas_acciones += "<td>" + fila.Responsable_Marcobre2 + "</td>";
                        else
                            filas_acciones += "<td>_</td>";
                        if (fila.Responsable_Contratista != null)
                            filas_acciones += "<td>" + fila.Responsable_Contratista + "</td>";
                        else
                            filas_acciones += "<td>_</td>";
                        filas_acciones += "<td>" + fila.Fecha.Value.Date.ToString("dd/MM/yyyy") + "</td>";
                        filas_acciones += "<td>" + fila.Estatus + "</td>";
                        //Estado
                        switch (fila.Estado)
                        {
                            case "Registrado": filas_acciones += "<td><span class=\"badge bg-secondary\">" + fila.Estado + "</span></td>"; break;
                            case "Aprobado": filas_acciones += "<td><span class=\"badge bg-success\">" + fila.Estado + "</span></td>"; break;
                            case "Rechazado": filas_acciones += "<td><span class=\"badge bg-danger\">" + fila.Estado + "</span></td>"; break;
                            case "Vencido": filas_acciones += "<td><span class=\"badge bg-danger\">" + fila.Estado + "</span></td>"; break;
                            case "Ampliado": filas_acciones += "<td><span class=\"badge bg-warning\">" + fila.Estado + "</span></td>"; break;
                            default: filas_acciones += "<td><span class=\"badge bg-secondary\">" + fila.Estado + "</span>"; break;
                        }
                        if (fila.Permiso_Verificacion && fila.Estatus.Equals("Cerrado") && (fila.Estado.Equals(EstadosIncidentes.Registrado.ToString()) || fila.Estado.Equals(EstadosIncidentes.Rechazado.ToString()) || fila.Estado.Equals(EstadosIncidentes.Ampliado.ToString())))
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc\" style=\"color:white;text-decoration: none;\" href=\"/ImplementarAccionMAIncidenteMenor/ImplementarAccionMA?IdAccion=" + IdEncriptado + "\">Cargar Evidencia" + "</a></td>";
                        else if (fila.Permiso_Implementacion && !fila.Estatus.Equals("Cerrado") && (fila.Estado.Equals(EstadosIncidentes.Registrado.ToString()) || fila.Estado.Equals(EstadosIncidentes.Rechazado.ToString()) || fila.Estado.Equals(EstadosIncidentes.Ampliado.ToString())))
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc\" style=\"color:white;text-decoration: none;\" href=\"/ImplementarAccionIncidenteMenor/ImplementarAccion?IdAccion=" + IdEncriptado + "\">Implementar</a></td>";
                        else if (fila.Permiso_Verificacion && fila.Estado.Equals(EstadosIncidentes.Implementado.ToString()))
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc\"style=\"color:white;text-decoration: none;\" href=\"/VerificarAccionIncidenteMenor/VerificarAccion?IdAccion=" + IdEncriptado + "\">Verificar</a></td>";
                        else if ((fila.Permiso_Verificacion || userInfo.Rol.Equals(InspeccionRol.Admin)) && fila.Estado.Equals(EstadosIncidentes.Vencido.ToString()))
                            filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc\" style=\"color:white;text-decoration: none;\" onclick=AmpliarFecha(" + fila.Id_Accion_Correctiva_Incidente + ")>Ampliar Fecha</button></td>";
                        else if (fila.Estado.Equals(EstadosIncidentes.Aprobado.ToString()))
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc-outline\" style=\"color:#2e6793;text-decoration: none;\" href=\"/VerificarAccionIncidenteMenor/DetalleAccion?IdAccion=" + IdEncriptado + "\">Detalle</a></td>";
                        else if ((fila.Permiso_Verificacion || userInfo.Rol.Equals(InspeccionRol.Admin)) && fila.Estado.Equals(EstadosIncidentes.Vencido.ToString()))
                            filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc\"style=\"color:white;text-decoration: none;\" onclick=\"AmpliarFecha(" + IdEncriptado + ")\">Ampliar Fecha</button></td>";
                        else
                            filas_acciones += "<td></td>";
                        filas_acciones += "</tr>";
                    }
                }
                else
                {
                    filas_acciones += "<tr>";
                    filas_acciones += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                    filas_acciones += "</tr>";
                }
                dynamic response = new
                {
                    filas_acciones = filas_acciones,
                    nAcciones = acciones.Count,
                    nVerificado = nVerificado
                };
                return Ok(response);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ListarAcciones(int IdReporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var acciones = await _getAccionesIncidente.Execute(IdReporte);
            if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                acciones = acciones.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var estadoReporte = await _getEstadoReportePreliminar.Execute(IdReporte);
            var filas_acciones_inmediatas = "";
            var filas_acciones_correctivas = "";
            if (acciones.Where(x => x.Tipo_Accion.Equals("Acción Inmediata")).Count() == 0)
            {
                filas_acciones_inmediatas += "<tr>";
                filas_acciones_inmediatas += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filas_acciones_inmediatas += "</tr>";
            }
            if (acciones.Where(x => x.Tipo_Accion.Equals("Acción Correctiva")).Count() == 0)
            {
                filas_acciones_correctivas += "<tr>";
                filas_acciones_correctivas += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filas_acciones_correctivas += "</tr>";
            }
            foreach (var fila in acciones)
            {
                if (fila.Tipo_Accion.Equals("Acción Inmediata"))
                {
                    filas_acciones_inmediatas += "<tr>";
                    if (fila.Accion_Inmediata != null)
                        filas_acciones_inmediatas += "<td>" + fila.Accion_Inmediata + "</td>";
                    else
                        filas_acciones_inmediatas += "<td>_</td>";
                    if (fila.Responsable_Marcobre != null)
                        filas_acciones_inmediatas += "<td>" + fila.Responsable_Marcobre + "</td>";
                    else
                        filas_acciones_inmediatas += "<td>_</td>";
                    if (fila.Responsable_Marcobre2 != null)
                        filas_acciones_inmediatas += "<td>" + fila.Responsable_Marcobre2 + "</td>";
                    else
                        filas_acciones_inmediatas += "<td>_</td>";
                    if (fila.Responsable_Contratista != null)
                        filas_acciones_inmediatas += "<td>" + fila.Responsable_Contratista + "</td>";
                    else
                        filas_acciones_inmediatas += "<td>_</td>";
                    filas_acciones_inmediatas += "<td>" + fila.Fecha.Value.Date.ToString("dd/MM/yyyy") + "</td>";
                    filas_acciones_inmediatas += "<td>" + fila.Estatus + "</td>";
                    if ((estadoReporte.Equals(EstadosIncidentes.Pendiente.ToString()) && userInfo.Rol.Equals(InspeccionRol.AsesorMAP)) || userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        filas_acciones_inmediatas += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarAccion(" + fila.Id_Accion_Correctiva_Incidente + ")\">Editar</button></td>";
                        filas_acciones_inmediatas += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarAccion(" + fila.Id_Accion_Correctiva_Incidente + ")\">Eliminar</button></td>";
                    }
                    else
                        filas_acciones_inmediatas += "<td></td><td></td>";
                    filas_acciones_inmediatas += "</tr>";
                }
                else if (fila.Tipo_Accion.Equals("Acción Correctiva"))
                {
                    filas_acciones_correctivas += "<tr>";
                    if (fila.Accion_Inmediata != null)
                        filas_acciones_correctivas += "<td>" + fila.Accion_Inmediata + "</td>";
                    else
                        filas_acciones_correctivas += "<td>_</td>";
                    if (fila.Responsable_Marcobre != null)
                        filas_acciones_correctivas += "<td>" + fila.Responsable_Marcobre + "</td>";
                    else
                        filas_acciones_correctivas += "<td>_</td>";
                    if (fila.Responsable_Marcobre2 != null)
                        filas_acciones_correctivas += "<td>" + fila.Responsable_Marcobre2 + "</td>";
                    else
                        filas_acciones_correctivas += "<td>_</td>";
                    if (fila.Responsable_Contratista != null)
                        filas_acciones_correctivas += "<td>" + fila.Responsable_Contratista + "</td>";
                    else
                        filas_acciones_correctivas += "<td>_</td>";
                    filas_acciones_correctivas += "<td>" + fila.Fecha.Value.Date.ToString("dd/MM/yyyy") + "</td>";
                    filas_acciones_correctivas += "<td>" + fila.Estatus + "</td>";
                    if ((estadoReporte.Equals(EstadosIncidentes.Pendiente.ToString()) && userInfo.Rol.Equals(InspeccionRol.AsesorMAP)) || userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        filas_acciones_correctivas += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarAccion(" + fila.Id_Accion_Correctiva_Incidente + ")\">Editar</button></td>";
                        filas_acciones_correctivas += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarAccion(" + fila.Id_Accion_Correctiva_Incidente + ")\">Eliminar</button></td>";
                    }
                    else
                        filas_acciones_correctivas += "<td></td><td></td>";
                    filas_acciones_correctivas += "</tr>";
                }
            }

            dynamic response = new
            {
                filas_acciones_inmediatas = filas_acciones_inmediatas,
                filas_acciones_correctivas = filas_acciones_correctivas,
                nAcciones = acciones.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevaAccion(int IdReporte, string tipoAccion, string accion, int usuario, int usuario2, string contratista, string fecha, string estado)
        {
            await _registrarAccionCorrectivaIncidente.Execute(IdReporte, tipoAccion, accion, usuario, usuario2, contratista, fecha, estado);
            return await ListarAcciones(IdReporte);
        }
        [HttpGet]
        public async Task<IActionResult> EditarAccion(int IdAccion)
        {
            var accion = await _getAccionIncidente.Execute(IdAccion);

            dynamic response = new
            {
                idAccion = accion.Id_Accion_Correctiva_Incidente,
                idReporte = accion.Id_Reporte_Preliminar,
                tipo = accion.Tipo_Accion,
                idUsuario = accion.Id_Usuario,
                responsableMarc = accion.Responsable_Marcobre,
                idUsuario2 = accion.Id_Usuario2,
                responsableMarc2 = accion.Responsable_Marcobre2,
                accion = accion.Accion_Inmediata,
                responsableCon = accion.Responsable_Contratista,
                fecha = accion.Fecha != null ? accion.Fecha.Value.ToString("yyyy-MM-dd") : "",
                estatus = accion.Estatus
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarAccion(int IdAccion, int IdReporte, string tipoAccion, int usuario, int usuario2, string accion, string contratista, string fecha, string estado)
        {
            await _editarAccionIncidente.Execute(IdAccion, IdReporte, tipoAccion, usuario, usuario2, accion, fecha, contratista, estado);
            return await ListarAcciones(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarAccion(int IdAccion, int IdReporte)
        {
            await _eliminarAccionIncidente.Execute(IdAccion);
            return await ListarAcciones(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> AmpliarFechaAccion(int IdAccion, int IdReporte, string fecha)
        {
            await _ampliarFechaAccion.Execute(IdAccion, fecha);
            return await ListarAccionesVerificacion(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> ListarCausas(int IdReporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var causas = await _getCausasIncidenteReporte.Execute(IdReporte);
            if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                causas = causas.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var estadoReporte = await _getEstadoReportePreliminar.Execute(IdReporte);
            var filas_causas_inmediatas = "";
            var filas_causas_basicas = "";

            //foreach (var fila in causas)
            //{
            //    filas_causas += "<tr>";
            //    filas_causas += "<td>" + fila.Tipo_Acto_Condicion + "</td>";
            //    filas_causas += "<td>" + fila.Acto_Condicion + "</td>";
            //    if (estadoReporte.Equals(EstadosIncidentes.Pendiente.ToString()) || userInfo.Rol.Equals(InspeccionRol.Admin))
            //    {
            //        filas_causas += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarCausa(" + fila.Id_Causa_Incidente + ")\">Editar</button></td>";
            //        filas_causas += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarCausa(" + fila.Id_Causa_Incidente + ")\">Eliminar</button></td>";
            //    }

            //    filas_causas += "</tr>";
            //}
            foreach (var fila in causas)
            {
                if (fila.Tipo_Acto_Condicion.Equals("Acto Subestándar") || fila.Tipo_Acto_Condicion.Equals("Condición Subestándar"))
                {
                    filas_causas_inmediatas += "<tr>";
                    filas_causas_inmediatas += "<td>" + fila.Tipo_Acto_Condicion + "</td>";
                    filas_causas_inmediatas += "<td>" + fila.Acto_Condicion + "</td>";
                    if (fila.Descripcion != null)
                        filas_causas_inmediatas += "<td>" + fila.Descripcion + "</td>";
                    else
                        filas_causas_inmediatas += "<td>_</td>";
                    if (estadoReporte.Equals(EstadosIncidentes.Pendiente.ToString()) || userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        filas_causas_inmediatas += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarCausa(" + fila.Id_Causa_Incidente + ")\">Editar</button></td>";
                        filas_causas_inmediatas += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarCausa(" + fila.Id_Causa_Incidente + ")\">Eliminar</button></td>";
                    }

                    filas_causas_inmediatas += "</tr>";
                }
                else if (fila.Tipo_Acto_Condicion.Equals("Factor Personal") || fila.Tipo_Acto_Condicion.Equals("Factor Laboral"))
                {
                    filas_causas_basicas += "<tr>";
                    filas_causas_basicas += "<td>" + fila.Tipo_Acto_Condicion + "</td>";
                    filas_causas_basicas += "<td>" + fila.Factor_Causa_Incidente + "</td>";
                    if (fila.Descripcion != null)
                        filas_causas_basicas += "<td>" + fila.Descripcion + "</td>";
                    else
                        filas_causas_basicas += "<td>_</td>";
                    if (estadoReporte.Equals(EstadosIncidentes.Pendiente.ToString()) || userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        filas_causas_basicas += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarCausa(" + fila.Id_Causa_Incidente + ")\">Editar</button></td>";
                        filas_causas_basicas += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarCausa(" + fila.Id_Causa_Incidente + ")\">Eliminar</button></td>";
                    }
                }
            }
            dynamic response = new
            {
                filas_causas_inmediatas = filas_causas_inmediatas,
                filas_causas_basicas = filas_causas_basicas,
                nCausas = causas.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevaCausa(int IdReporte, int IdActoCondicion, string Acto_Condicion, string Descripcion)
        {
            await _registrarCausaIncidenteReporte.Execute(IdReporte, IdActoCondicion, Acto_Condicion, Descripcion);
            return await ListarCausas(IdReporte);
        }
        [HttpGet]
        public async Task<IActionResult> EditarCausa(int IdCausa)
        {
            var causa = await _getCausaIncidenteReporte.Execute(IdCausa);
            dynamic response = new
            {
                idCausa = causa.Id_Causa_Incidente,
                idActoCondicion = causa.Id_Acto_Condicion,
                idFactor = causa.Id_Factor_Causa_Incidente,
                tipoActoCondicion = causa.Tipo_Acto_Condicion,
                descripcion = causa.Descripcion
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarCausa(int IdCausa, int IdReporte, int IdActoCondicion, string Acto_Condicion, string Descripcion)
        {
            await _editarCausaIncidenteReporte.Execute(IdCausa, IdReporte, IdActoCondicion, Acto_Condicion, IdActoCondicion, Descripcion);
            return await ListarCausas(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarCausa(int IdCausa, int IdReporte)
        {
            await _eliminarCausaIncidenteReporte.Execute(IdCausa);
            return await ListarCausas(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> NuevoReporteParte2(ReporteParte2Model model)
        {
            if (ModelState.IsValid)
            {
                var nombresEvidencias = await _getEvidenciasReporte.Execute(model.Id_Reporte_Preliminar);
                var nombresArchivos = JsonConvert.DeserializeObject<List<NombreEvidencia>>(nombresEvidencias);

                var evidencias = new List<string>();
                var evidenciasNombres = new List<NombreEvidencia>();

                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias[i].FileName);
                        var max_file_size = 12000000;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                            evidencias.Add(filePath);

                            evidenciasNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }
                //var evidencias = new List<string>();
                //var max_file_size = 12000000;

                //if (model.Evidencias != null)
                //{
                //    for (int i = 0; i < model.Evidencias.Count; i++)
                //    {
                //        var extension = Path.GetExtension(model.Evidencias[i].FileName);

                //        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG"};

                //        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                //        {
                //            var newfileName = CreateUniqueFileName(i+1) + ".zip";
                //            var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                //            evidencias.Add(filePath);
                //        }
                //    }
                //}
                await _registrarReporteParte2.Execute(
                    model.Id_Reporte_Preliminar,
                    model.Otra_Causa,
                    model.Id_Gerencia_Responsable,
                    evidencias,
                    JsonConvert.SerializeObject(evidenciasNombres),
                    model.Comentario_Accion,
                    model.Id_Supervisor,
                    model.Opcion);
                if (model.Opcion.Equals("Completar"))
                    return RedirectToAction("NotificarNuevoReporte", "NotificacionesReporteIncidente", new { IdReporte = model.Id_Reporte_Preliminar });
                else
                    return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoReporteParte2", new { IdReporte = model.Id_Reporte_Preliminar });
        }
        public async Task<IActionResult> EditarReportePreliminar(string IdReporte)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdReporte=" + IdReporte });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                ViewBag.PermisosAdmin = userInfo.Rol.Equals(InspeccionRol.Admin);

                var items = await _selectItems.GetSelectItems();

                var tipo_Incidente = items.ItemsTipoIncidente.Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
                tipo_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nivel_Incidente = items.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
                nivel_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Derramado = items.ItemsUnidadMedida.Where(
                   x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                   x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()) ||
                   x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                   .OrderBy(x => x.Descripcion)
                   .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true)); ;

                var unidad_Area = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Area.ToString()))
                    .OrderBy(x => x.Descripcion)
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Area.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var componente_Ambiental = items.ItemsCompAmb.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Componente_Ambiental.ToString(), Text = g.Descripcion }).ToList();
                componente_Ambiental.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //Causa Incidente
                var grupoActo = new SelectListGroup() { Name = "Acto Subestándar", Disabled = false };
                var grupoCondicion = new SelectListGroup() { Name = "Condición Subestándar", Disabled = false };

                var causa_Incidente = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoActo }).ToList();
                causa_Incidente.AddRange(items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoCondicion }).ToList());
                causa_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var gerencias = items.ItemsGerencia.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var supervisores = items.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
                supervisores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
                usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var actos = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                actos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var condiciones = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var factoresPersonales = items.ItemsFactores.Where(x => x.Tipo_Factor.Equals("Factor Personal")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Factor_Causa_Incidente.ToString(), Text = g.Descripcion }).ToList();
                factoresPersonales.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var factoresLaborales = items.ItemsFactores.Where(x => x.Tipo_Factor.Equals("Factor Laboral")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Factor_Causa_Incidente.ToString(), Text = g.Descripcion }).ToList();
                factoresLaborales.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Tipo_Incidente = tipo_Incidente;
                ViewBag.Nivel_Incidente = nivel_Incidente;
                ViewBag.Unidad_Derramado = unidad_Derramado;
                ViewBag.Unidad_Area = unidad_Area;
                ViewBag.Componente_Ambiental = componente_Ambiental;
                ViewBag.Causa_Incidente = causa_Incidente;
                ViewBag.Gerencias = gerencias;
                ViewBag.Supervisor = supervisores;
                ViewBag.Usuario = usuario;
                ViewBag.Actos = actos;
                ViewBag.Condiciones = condiciones;
                ViewBag.FactoresPersonales = factoresPersonales;
                ViewBag.FactoresLaborales = factoresLaborales;

                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdReporte));

                var reporte = await _getReportePreliminar.Execute(IdDesencriptado,userInfo.NombreUsuario);
                var nombresEvidencias = await _getEvidenciasReporte.Execute(IdDesencriptado);
                var model = new EditarReporteModel();
                model.Id_Encriptado_Reporte_Preliminar = IdReporte;
                model.Id_Reporte_Preliminar = reporte.Id_Reporte_Preliminar;
                model.Codigo = reporte.Codigo;
                model.Num_Trabajadores = reporte.Num_Trabajadores;
                model.Razon_Social_Contratista = reporte.Razon_Social_Contratista;
                model.RUC_Contratista = reporte.RUC_Contratista;
                model.Actividad_Contratista = reporte.Actividad_Contratista;
                model.Domicilio_Contratista = reporte.Domicilio_Contratista;
                model.Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;
                model.Id_Tipo_Incidente = reporte.Id_Tipo_Incidente.HasValue ? reporte.Id_Tipo_Incidente.Value : 0;
                model.Id_Nivel_Incidente = reporte.Id_Nivel_Incidente.HasValue ? reporte.Id_Nivel_Incidente.Value : 0;
                model.Fecha_Incidente = reporte.Fecha_Incidente != null ? reporte.Fecha_Incidente.Value.ToString("yyyy-MM-dd") : "";
                model.Hora_Incidente = reporte.Fecha_Incidente != null ? reporte.Fecha_Incidente.Value.ToString("HH:mm") : "";
                model.Producto_Derramado = reporte.Producto_Derramado;
                model.Cantidad_Derramado = reporte.Cantidad_Derramado;
                model.Id_Unidad_Cantidad_Derramado = reporte.Id_Unidad_Cantidad_Derramado.HasValue ? reporte.Id_Unidad_Cantidad_Derramado.Value : 0;
                model.Area_Afectada = reporte.Area_Afectada;
                model.Id_Unidad_Area_Afectada = reporte.Id_Unidad_Area_Afectada.HasValue ? reporte.Id_Unidad_Area_Afectada.Value : 0;
                model.Lugar_Incidente = reporte.Lugar_Incidente;
                model.Equipo_Involucrado = reporte.Equipo_Involucrado;
                model.Id_Componente_Ambiental = reporte.Id_Componente_Ambiental.HasValue ? reporte.Id_Componente_Ambiental.Value : 0;
                model.Descripcion_Incidente = reporte.Descripcion_Incidente;
                model.Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;
                model.Id_Gerencia_Responsable = reporte.Id_Gerencia_Responsable;
                model.Comentario_Accion = reporte.Comentario_Accion;
                model.Id_Supervisor = reporte.Id_Supervisor_MA;
                model.Evidencias_Completas = reporte.Evidencia != null ? JsonConvert.SerializeObject(reporte.Evidencia) : null;
                model.Evidencias_Nombres = nombresEvidencias;
                model.Estado = reporte.Estado;
                model.Permiso_Editar = reporte.PermisoEditar;

                ViewBag.TieneInvestigacion = reporte.Investigacion;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarReportePreliminar(EditarReporteModel model)
        {
            //if (ModelState.IsValid)
            //{
                var nombresEvidencias = await _getEvidenciasReporte.Execute(model.Id_Reporte_Preliminar);
                var nombresArchivos = JsonConvert.DeserializeObject<List<NombreEvidencia>>(nombresEvidencias);

                var evidencias = new List<string>();
                var evidenciasNombres = new List<NombreEvidencia>();

                //Existen evidencias anteriores
                if (model.Evidencias_Completas != null)
                {
                    var temp = new List<NombreEvidencia>();
                    evidencias.AddRange(new List<string>(model.Evidencias_Completas.Split(',')));

                    //Validar nombres archivos tiene categoria papel
                    if (nombresArchivos.Count > 0)
                    {
                        //Eliminar las evidencias que no se encuentre en la lista anterior
                        temp = nombresArchivos.Where(x => evidencias.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene datos, agregar los existentes
                        if (evidencias != null)
                        {
                            temp = evidencias.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    evidenciasNombres.AddRange(temp);
                }
                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias[i].FileName);
                        var max_file_size = 12000000;

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1) + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                            evidencias.Add(filePath);

                            evidenciasNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                await _editarReportePreliminar.Execute(
                    model.Id_Reporte_Preliminar,
                    model.Num_Trabajadores,
                    model.Razon_Social_Contratista,
                    model.RUC_Contratista,
                    model.Actividad_Contratista,
                    model.Domicilio_Contratista,
                    model.Num_Trabajadores_Contratista,
                    model.Id_Tipo_Incidente,
                    model.Id_Nivel_Incidente,
                    model.Fecha_Incidente,
                    model.Hora_Incidente,
                    model.Producto_Derramado,
                    model.Cantidad_Derramado,
                    model.Id_Unidad_Cantidad_Derramado,
                    model.Area_Afectada,
                    model.Id_Unidad_Area_Afectada,
                    model.Lugar_Incidente,
                    model.Equipo_Involucrado,
                    model.Id_Componente_Ambiental,
                    model.Descripcion_Incidente,
                    model.Otra_Causa_Incidente,
                    model.Id_Gerencia_Responsable,
                    evidencias,
                    JsonConvert.SerializeObject(evidenciasNombres),
                    model.Comentario_Accion,
                    model.Id_Supervisor,
                    model.Opcion);

            if (model.Opcion.Equals("Completar"))
                return RedirectToAction("NotificarNuevoReporte", "NotificacionesReporteIncidente", new { IdReporte = model.Id_Reporte_Preliminar });
            else
                return RedirectToAction("EditarReportePreliminar", new { IdReporte = model.Id_Encriptado_Reporte_Preliminar });
        }
        [HttpPost]
        public async Task<IActionResult> EliminarReportePreliminar(string IdReporte)
        {
            dynamic response;

            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (userInfo.Rol.Equals(InspeccionRol.Admin))
                {
                    var IdDesenc = Convert.ToInt32(_protector.Unprotect(IdReporte));
                    await _eliminarReportePreliminar.Execute(IdDesenc);
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
                        message = "No está autorizado"
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
        //Carga y descarga de archivos
        [HttpPost]
        public async Task<IActionResult> EliminarEvidencia(int IdReporte, string Evidencia)
        {
            dynamic response;
            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var eliminado = await _eliminarEvidencia.Execute(IdReporte, Evidencia, userInfo.NombreUsuario);
                var reportePreliminar = await _getReportePreliminar.Execute(IdReporte);
                var nombresEvidencias = JsonConvert.DeserializeObject<List<NombreEvidencia>>(reportePreliminar.Nombres_Archivos);
                //var evidencia = await _getEvidenciaInvestigacion.Execute(IdFlashReport, Tipo);
                try
                {
                    if (eliminado)
                    {
                        var directorio = Tipo_Info_SGA.FTPConnectionEvidenciaFlashReport;
                        await _gestorArchivos.DeleteFTPFileAsync(Evidencia, directorio);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                    Log.Error(e.Message + " - Archivo: " + Evidencia);
                }
                if (eliminado)
                    response = new
                    {
                        success = true,
                        nombresEvidencias = nombresEvidencias,
                        evidencia = nombresEvidencias.Select(x => x.NombreCarga).ToList()
                    };
                else
                    response = new
                    {
                        success = false,
                        message = "No se eliminó la evidencia"
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
        [HttpPost]
        public async Task<IActionResult> ObtenerImagenesCargadas(string imagenes)
        {
            dynamic response;
            if (HttpContext.Session.GetString("SessionUser") != null && imagenes != null)
            {
                var listaImagenes = new List<string>(imagenes.Split(','));
                var archivos = JsonConvert.DeserializeObject<List<NombreEvidencia>>(imagenes);
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var imagesSrc = new List<ImagenCargada>();

                for (int i = 0; i < archivos.Count; i++)
                {
                    try
                    {
                        var encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(archivos[i].NombreCarga, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaIncR);
                        /* Save encrypted file in temp path*/
                        var tempPath = Path.GetTempPath();

                        var SGATempPath = tempPath + "Descarga Temporal Gestiona V2\\";
                        if (!Directory.Exists(SGATempPath))
                        {
                            Directory.CreateDirectory(SGATempPath);
                        }
                        var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                        var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                        /*decrypt file*/
                        var decryptedFileName = "desencriptado" + i.ToString() + Path.GetExtension(archivos[i].NombreCarga);
                        await _gestorEncriptado.DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
                        /*unzip file*/
                        if (!filePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                            filePath += Path.DirectorySeparatorChar;

                        using (ZipArchive archive = ZipFile.OpenRead(archivePath + decryptedFileName))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                filePath = Path.GetFullPath(Path.Combine(filePath, entry.FullName));
                                if (filePath.StartsWith(filePath, StringComparison.Ordinal))
                                    entry.ExtractToFile(filePath);
                            }
                        }
                        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                        var newFileName = archivos[i].NombreCarga.Replace(Path.GetExtension(archivos[i].NombreCarga), Path.GetExtension(filePath));
                        /*read decrypted file*/
                        //byte[] imageByteData = System.IO.File.ReadAllBytes(archivePath + decryptedFileName);
                        string imageBase64Data = Convert.ToBase64String(bytes);
                        string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                        imagesSrc.Add(new ImagenCargada
                        {
                            Nombre = archivos[i].NombreOrigen,
                            NombreCarga = archivos[i].NombreCarga,
                            Src = imageDataURL,
                            EsImagen = (archivos[i].ExtOrigen.ToLower() == ".jpg" || archivos[i].ExtOrigen.ToLower() == ".jpeg" || archivos[i].ExtOrigen.ToLower() == ".png" || archivos[i].ExtCarga.ToLower() == ".jpg" || archivos[i].ExtCarga.ToLower() == ".jpeg" || archivos[i].ExtCarga.ToLower() == ".png")
                        });
                    }
                    catch (Exception ex)
                    {
                        //No se encontró el archivo 
                    }
                }
                response = new
                {
                    imagenes = JsonConvert.SerializeObject(imagesSrc)
                };
            }
            else
            {
                response = new
                {
                    imagenes = "[]"
                };
            }
            return Ok(response);
        }
        private string CreateUniqueFileName(int index)
        {
            return "Ev_Inc_Rep_Pre" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + index;
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
                    var encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(fileName, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaIncR);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "Descarga Temporal Gestiona V2\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = archivePath + "desencriptado.zip";
                    await _gestorEncriptado.DecryptFileAsync(encryptedFile, decryptedFileName);
                    //descomprimir en carpeta temporal submission
                    if (!filePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                        filePath += Path.DirectorySeparatorChar;
                    var outputFileName = "";
                    using (ZipArchive archive = ZipFile.OpenRead(decryptedFileName))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            outputFileName = entry.FullName;
                            filePath = Path.GetFullPath(Path.Combine(filePath, entry.FullName));
                            if (filePath.StartsWith(filePath, StringComparison.Ordinal))
                                entry.ExtractToFile(filePath);
                        }
                    }
                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                    var newFileName = fileName.Replace(Path.GetExtension(fileName), Path.GetExtension(filePath));
                    ///*read decrypted file*/
                    //FileStream stream = new FileStream(decryptedFileName, FileMode.Open);

                    return File(bytes, GetContentType(filePath), outputFileName!=""? outputFileName:newFileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
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
                var SGATempPath = tempPath + "Descarga Temporal Gestiona V2/";
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
                await _gestorEncriptado.EncryptFileAsync(archiveFile, encryptedArchiveFile);

                return await _gestorArchivos.UploadFTPFileAsync(encryptedArchiveFile, newFileName, Tipo_Info_SGA.FTPConnectionEvidenciaIncR);
            }
        }
        private class NombreEvidencia
        {
            public string NombreOrigen { get; set; }
            public string NombreCarga { get; set; }
            public string ExtOrigen { get; set; }
            public string ExtCarga { get; set; }
        }
        private class ImagenCargada
        {
            public string Nombre { get; set; }
            public string Src { get; set; }
            public string NombreCarga { get; set; }
            public bool EsImagen { get; set; }
        }
    }
}