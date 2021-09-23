using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.RegistrarInvestigacionFinal;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EditarReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEstadoReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.GetReportesPreliminares;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EditarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EliminarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.RegistrarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EliminarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EditarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.RegistrarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.EditarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.EliminarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.GetInvolucrados;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.RegistrarInvolucrado;
using Microsoft.AspNetCore.DataProtection;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.EliminarEvindenciaReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.GetEvidenciaReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetEvidenciasReporte;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using GestionAmbiental.UseCases.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.AgregarEvidenciaAccionReporte;

namespace GestionAmbiental.UseCases.Incidentes.ReportePreliminarMA
{
    public class ReportePreliminarMAController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly ISelectItems _selectItems;
        private readonly IGetEvidenciasReporte _getEvidenciasReporte;
        private readonly IGetReportesPreliminaresMA _getReportesPreliminares;
        private readonly IGetReportePreliminar _getReportePreliminar;
        private readonly IGetEstadoReportePreliminar _getEstadoReportePreliminar;
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
        private readonly IGetCausasIncidenteReporte _getCausasIncidenteReporte;
        private readonly IGetCausaIncidenteReporte _getCausaIncidenteReporte;
        private readonly IEditarCausaIncidenteReporte _editarCausaIncidenteReporte;
        private readonly IEliminarCausaIncidenteReporte _eliminarCausaIncidenteReporte;
        private readonly IRegistrarCausaIncidenteReporte _registrarCausaIncidenteReporte;
        private readonly IEditarReportePreliminar _editarReportePreliminar;
        private readonly IRegistrarInvestigacionFinal _registrarInvestigacionFinal;
        private readonly IEliminarEvindenciaReportePreliminar _eliminarEvindenciaReportePreliminar;
        private readonly IGetEvidenciaReportePreliminar _getEvidenciaReportePreliminar;
        private readonly IAgregarEvidenciaAccionReporte _agregarEvidenciaAccionReporte;
        private readonly IGetInfo_SGA _info_SGA;

        private readonly IDataProtector _protector;
        public ReportePreliminarMAController(
            IWebHostEnvironment hostingEnvironment,
            ISelectItems selectItems, 
            IGetEvidenciasReporte getEvidenciasReporte,
            IGetReportesPreliminaresMA getReportesPreliminares,
            IGetReportePreliminar getReportePreliminar,
            IGetEstadoReportePreliminar getEstadoReportePreliminar,
            IGetInvolucradosIncidente getInvolucradosIncidente,
            IGetInvolucradoIncidente getInvolucradoIncidente,
            IEditarInvolucradoIncidente editarInvolucradoIncidente,
            IEliminarInvolucradoIncidente eliminarInvolucradoIncidente,
            IRegistrarInvolucradoIncidente registrarInvolucradoIncidente,
            IRegistrarAccionCorrectivaIncidente registrarAccionCorrectivaIncidente,
            IEditarAccionIncidente editarAccionIncidente,
            IGetAccioIncidente getAccionIncidente,
            IEliminarAccionIncidente eliminarAccionIncidente,
            IGetAccionesIncidente getAccionesIncidente,
            IGetCausasIncidenteReporte getCausasIncidenteReporte,
            IGetCausaIncidenteReporte getCausaIncidenteReporte,
            IEditarCausaIncidenteReporte editarCausaIncidenteReporte,
            IEliminarCausaIncidenteReporte eliminarCausaIncidenteReporte,
            IRegistrarCausaIncidenteReporte registrarCausaIncidenteReporte,
            IEditarReportePreliminar editarReportePreliminar,
            IRegistrarInvestigacionFinal registrarInvestigacionFinal,
            IEliminarEvindenciaReportePreliminar eliminarEvindenciaReportePreliminar,
            IGetEvidenciaReportePreliminar getEvidenciaReportePreliminar, 
            IAgregarEvidenciaAccionReporte agregarEvidenciaAccionReporte,
            IDataProtectionProvider dataProtectionProvider, 
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _hostingEnvironment = hostingEnvironment;
            _selectItems = selectItems;
            _getEvidenciasReporte = getEvidenciasReporte;
            _getReportesPreliminares = getReportesPreliminares;
            _getReportePreliminar = getReportePreliminar;
            _getEstadoReportePreliminar = getEstadoReportePreliminar;
            _getInvolucradosIncidente = getInvolucradosIncidente;
            _getInvolucradoIncidente = getInvolucradoIncidente;
            _editarInvolucradoIncidente = editarInvolucradoIncidente;
            _eliminarInvolucradoIncidente = eliminarInvolucradoIncidente;
            _registrarInvolucradoIncidente = registrarInvolucradoIncidente;
            _registrarAccionCorrectivaIncidente = registrarAccionCorrectivaIncidente;
            _editarAccionIncidente = editarAccionIncidente;
            _getAccionIncidente = getAccionIncidente;
            _eliminarAccionIncidente = eliminarAccionIncidente;
            _getAccionesIncidente = getAccionesIncidente;
            _getCausasIncidenteReporte = getCausasIncidenteReporte;
            _getCausaIncidenteReporte = getCausaIncidenteReporte;
            _editarCausaIncidenteReporte = editarCausaIncidenteReporte;
            _eliminarCausaIncidenteReporte = eliminarCausaIncidenteReporte;
            _registrarCausaIncidenteReporte = registrarCausaIncidenteReporte;
            _editarReportePreliminar = editarReportePreliminar;
            _registrarInvestigacionFinal = registrarInvestigacionFinal;
            _eliminarEvindenciaReportePreliminar = eliminarEvindenciaReportePreliminar;
            _getEvidenciaReportePreliminar = getEvidenciaReportePreliminar;
            _agregarEvidenciaAccionReporte = agregarEvidenciaAccionReporte;
            _info_SGA = info_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.ReportePreliminarIdRouteValue);
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

                var asesores = items.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuarios = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).OrderBy(x => x.Text).ToList();
                usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Asesores = asesores;
                ViewBag.Gerencias = gerencias;
                ViewBag.Usuarios = usuarios;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha, int id_asesor, int id_usuario, int id_gerencia, int rpp, int pagina)
        {
            var datosTabla = await _getReportesPreliminares.Execute(fecha, id_asesor, id_usuario, id_gerencia, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NReportes % rpp != 0 ? (datosTabla.NReportes / rpp) + 1 : datosTabla.NReportes / rpp;
            if (datosTabla.NReportes > 0)
            {
                foreach (var fila in datosTabla.Reportes)
                {
                    var IdEncriptado = _protector.Protect(fila.Id_Reporte_Preliminar.ToString());
                    filasTabla += "<tr>";
                    if(fila.Estado.Equals(EstadosIncidentes.Registrado.ToString()))
                        filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                    else
                        filasTabla += "<td></td>";
                    filasTabla += "<td style=\"width: 115px;\"><a href=\"/ReportePreliminarMA/EditarReportePreliminarMA?IdReporte=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
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
                var nCol = 11;
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
        public async Task<IActionResult> EditarReportePreliminarMA(string IdReporte) 
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

                var gerencias = items.ItemsGerencia.Where(x=>x.Estado.Equals("ACT")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var supervisores = items.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                supervisores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var actos = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                actos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var condiciones = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
                condiciones.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

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

                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdReporte));

                var reporte = await _getReportePreliminar.Execute(IdDesencriptado);
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

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarReportePreliminarMA(EditarReporteModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (ModelState.IsValid)
                {
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
                    //var evidencias = new List<string>();
                    //if (model.Evidencias != null)
                    //{
                    //    for (int i = 0; i < model.Evidencias.Count; i++)
                    //    {
                    //        var extension = Path.GetExtension(model.Evidencias[i].FileName);
                    //        var max_file_size = 8388608;

                    //        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                    //        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                    //        {
                    //            var newfileName = CreateUniqueFileName(i+1) + ".zip";
                    //            var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                    //            evidencias.Add(filePath);
                    //        }
                    //    }
                    //}
                    //if (model.Evidencias_Completas != null)
                    //{
                    //    evidencias.AddRange(new List<string>(model.Evidencias_Completas.Split(',')));
                    //}
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
                    if (model.Opcion.Equals("Difundir") && model.Investigacion)
                        return RedirectToAction("IniciarInvestigacion", new { IdReporte = model.Id_Encriptado_Reporte_Preliminar });
                    else if(model.Opcion.Equals("Difundir"))
                        return RedirectToAction("DifundirNuevoReporte", "NotificacionesReporteIncidente", new { IdReporte = model.Id_Reporte_Preliminar });
                    else
                        return RedirectToAction("Resumen");
                }
                return RedirectToAction("EditarReportePreliminar", new { IdReporte = model.Id_Encriptado_Reporte_Preliminar });
            }
        }
        public async Task<IActionResult> IniciarInvestigacion(string IdReporte)
        {
            int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdReporte));
            var id_investigacion_final = await _registrarInvestigacionFinal.Execute(IdDesencriptado);
            return RedirectToAction("DifundirNuevoReporte", "NotificacionesReporteIncidente", new { IdReporte = IdDesencriptado });
        }
        [HttpPost]
        public async Task<IActionResult> IniciarInvestigacionSinNotificar(int IdReporte)
        {
            try
            {
                var id_investigacion_final = await _registrarInvestigacionFinal.Execute(IdReporte);
                return Ok("Investigación Generada");
            }catch(Exception e)
            {
                return Ok(e.Message);
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
                filas_involucrados += "<td>" + fila.Nombres + " " + fila.Apellidos + "</td>";
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
                if (estadoReporte.Equals(EstadosIncidentes.Registrado.ToString()))
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
                nombres = involucrado.Nombres,
                apellidos = involucrado.Apellidos,
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
        public async Task<IActionResult> ListarAcciones(int IdReporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var acciones = await _getAccionesIncidente.Execute(IdReporte);
            if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                acciones = acciones.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var estadoReporte = await _getEstadoReportePreliminar.Execute(IdReporte);
            var filas_acciones = "";
            foreach (var fila in acciones)
            {
                filas_acciones += "<tr>";
                if (fila.Accion_Inmediata != null)
                    filas_acciones += "<td>" + fila.Accion_Inmediata + "</td>";
                else
                    filas_acciones += "<td>_</td>";
                if (fila.Responsable_Marcobre != null)
                    filas_acciones += "<td>" + fila.Responsable_Marcobre + "</td>";
                else
                    filas_acciones += "<td>_</td>";
                if (fila.Responsable_Contratista != null)
                    filas_acciones += "<td>" + fila.Responsable_Contratista + "</td>";
                else
                    filas_acciones += "<td>_</td>";
                filas_acciones += "<td>" + fila.Fecha.Value.Date.ToString("dd/MM/yyyy") + "</td>";
                filas_acciones += "<td>" + fila.Estatus + "</td>";
                if (estadoReporte.Equals(EstadosIncidentes.Registrado.ToString()))
                {
                    filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarAccion(" + fila.Id_Accion_Correctiva_Incidente + ")\">Editar</button></td>";
                    filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarAccion(" + fila.Id_Accion_Correctiva_Incidente + ")\">Eliminar</button></td>";
                }
                else if ((estadoReporte.Equals(EstadosIncidentes.Difundido.ToString()) || estadoReporte.Equals(EstadosIncidentes.PendienteInvestigacion.ToString())) && fila.Estatus.Equals("En Proceso"))
                {
                    filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarAccionDifundido(" + fila.Id_Accion_Correctiva_Incidente + ",\'" + fila.Estatus + "\')\">Editar</button></td>";
                }
                filas_acciones += "</tr>";
            }
            dynamic response = new
            {
                filas_acciones = filas_acciones,
                nAcciones = acciones.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevaAccion(int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado)
        {
            await _registrarAccionCorrectivaIncidente.Execute(IdReporte, descripcion, usuario, contratista, fecha, estado);
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
                idUsuario = accion.Id_Usuario,
                responsableMarc = accion.Responsable_Marcobre,
                accion = accion.Accion_Inmediata,
                responsableCon = accion.Responsable_Contratista,
                fecha = accion.Fecha != null ? accion.Fecha.Value.ToString("yyyy-MM-dd") : "",
                estatus = accion.Estatus
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarAccion(int IdAccion, int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado)
        {
            await _editarAccionIncidente.Execute(IdAccion, IdReporte, descripcion, usuario, contratista, fecha, estado);
            return await ListarAcciones(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> EditarAccionDifundido(int IdAccion, int IdReporte, string Estatus, IFormFile Evidencia,string EvidenciasCompletas)
        {
            dynamic response;
            try
            {
                await _editarAccionIncidente.Execute(IdAccion, IdReporte, Estatus);
                var nombresEvidencias = await _getEvidenciasReporte.Execute(IdReporte);
                var nombresArchivos = JsonConvert.DeserializeObject<List<NombreEvidencia>>(nombresEvidencias);

                var evidencias = new List<string>();
                var evidenciasNombres = new List<NombreEvidencia>();

                //Existen evidencias anteriores
                if (EvidenciasCompletas != null)
                {
                    var temp = new List<NombreEvidencia>();
                    evidencias.AddRange(new List<string>(EvidenciasCompletas.Split(',')));

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
                if (Evidencia != null)
                {
                    var extension = Path.GetExtension(Evidencia.FileName);
                    var max_file_size = 12582912;

                    var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

                    if (allowed_extensions.Contains(extension) && Evidencia.Length < max_file_size)
                    {
                        var newfileName = CreateUniqueFileNameAccion() + ".zip";
                        var filePath = await UploadZipFileAsync(Evidencia, newfileName);
                        evidencias.Add(filePath);

                        evidenciasNombres.Add(new NombreEvidencia
                        {
                            NombreOrigen = Evidencia.FileName,
                            NombreCarga = filePath,
                            ExtOrigen = Path.GetExtension(Evidencia.FileName),
                            ExtCarga = Path.GetExtension(filePath)
                        });
                    }
                }

                var evidenciaAgregada = await _agregarEvidenciaAccionReporte.Execute(IdReporte, JsonConvert.SerializeObject(evidencias), JsonConvert.SerializeObject(evidenciasNombres));
                if (evidenciaAgregada)
                    response = new
                    {
                        success = true,
                        nombresEvidencias = JsonConvert.SerializeObject(evidenciasNombres)
                    };
                else
                    response = new
                    {
                        success = false,
                        message = "Error al agregar evidencia"
                    };
            }
            catch(Exception e)
            {
                response = new
                {
                    success = false,
                    message = "Error al editar acción: "+e.Message
                };
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarAccion(int IdAccion, int IdReporte)
        {
            await _eliminarAccionIncidente.Execute(IdAccion);
            return await ListarAcciones(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> ListarCausas(int IdReporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var causas = await _getCausasIncidenteReporte.Execute(IdReporte);
            if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                causas = causas.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var estadoReporte = await _getEstadoReportePreliminar.Execute(IdReporte);
            var filas_causas = "";
            foreach (var fila in causas)
            {
                filas_causas += "<tr>";
                filas_causas += "<td>" + fila.Tipo_Acto_Condicion + "</td>";
                filas_causas += "<td>" + fila.Acto_Condicion + "</td>";
                if (estadoReporte.Equals(EstadosIncidentes.Registrado.ToString()))
                {
                    filas_causas += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarCausa(" + fila.Id_Causa_Incidente + ")\">Editar</button></td>";
                    filas_causas += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarCausa(" + fila.Id_Causa_Incidente + ")\">Eliminar</button></td>";
                }

                filas_causas += "</tr>";
            }
            dynamic response = new
            {
                filas_causas = filas_causas,
                nCausas = causas.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevaCausa(int IdReporte, int IdActoCondicion, string Acto_Condicion)
        {
            await _registrarCausaIncidenteReporte.Execute(IdReporte, IdActoCondicion, Acto_Condicion);
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
                tipoActoCondicion = causa.Tipo_Acto_Condicion
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarCausa(int IdCausa, int IdReporte, int IdActoCondicion, string Acto_Condicion)
        {
            await _editarCausaIncidenteReporte.Execute(IdCausa, IdReporte, IdActoCondicion, Acto_Condicion);
            return await ListarCausas(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarCausa(int IdCausa, int IdReporte)
        {
            await _eliminarCausaIncidenteReporte.Execute(IdCausa);
            return await ListarCausas(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarEvidencia(int IdReporte, string Evidencia)
        {
            dynamic response;
            try
            {
                var eliminado = await _eliminarEvindenciaReportePreliminar.Execute(IdReporte, Evidencia);
                var nueva_evidencia = await _getEvidenciaReportePreliminar.Execute(IdReporte);
                var nombresEvidencias = await _getEvidenciasReporte.Execute(IdReporte);
                try
                {
                    if (eliminado)
                        await DeleteFTPFileAsync(Evidencia);
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
                        evidencia = nueva_evidencia,
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

        //Carga y descarga de archivos
        private string CreateUniqueFileName(int index)
        {
            return "Ev_Inc_Rep_Pre" + DateTime.UtcNow.ToString("yyyyMMddHHmmss")+"_"+index;
        }
        private string CreateUniqueFileNameAccion()
        {
            return "Ev_Inc_Rep_Pre_Accion_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
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
            /*if (!(HttpContext.Session.GetString("SessionUser") != null))
                return "NoAutorizado";
            else
            {*/
            Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncR;

            var serverInfo = await _info_SGA.Execute(directorio);

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
            //}
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
                Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncR;

                var serverInfo = await _info_SGA.Execute(directorio);
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

            foreach (var item in dataToEncrypt.Info)
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
        private async Task DeleteFTPFileAsync(string filename)
        {
            Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncR;

            var serverInfo = await _info_SGA.Execute(directorio);
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
            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);

                sftp.DeleteFile(filename);

                sftp.Dispose();
            }
        }
    }
}