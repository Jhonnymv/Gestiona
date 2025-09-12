using GestionAmbiental.Application.UseCases.GestorUsuarios.GetRolUsuario;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.AmpliarFechaAccion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.EditarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.EliminarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.RegistrarAccion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.EditarCasusa;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.EliminarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.GetCausa;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.RegistrarCausa;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EditarAdminInvestigacion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EditarInvestigacionFinal;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EliminarEvidenciaInvestigacion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEstadoInvestigacion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEvidenciaInvestigacion;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEvidenciasNombre;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetImpactoAmbiental;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionesFinales;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionFinal;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.EditarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.EliminarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.GetInvolucrados;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.RegistrarInvolucrado;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.EditarProducto;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.EliminarProducto;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.GetProductos;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.RegistrarProductoDerramado;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.RegistrarInvestigacionFinal;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using GestionAmbiental.UseCases.Util.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HPSF;
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
using static GestionAmbiental.UseCases.InspeccionesAmbientales.DescargarInspeccion.DescargaInspeccionController;

namespace GestionAmbiental.UseCases.Incidentes.InvestigacionFinal
{
    public class InvestigacionFinalController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarInvestigacionFinal _registrarInvestigacionFinal;
        private readonly IGetInvestigacionesFinales _getInvestigacionesFinales;
        private readonly IGetInvestigacionFinal _getInvestigacionFinal;
        private readonly IGetEstadoInvestigacion _getEstadoInvestigacion;
        private readonly IGetImpactoAmbiental _getImpactoAmbiental;
        private readonly IEditarInvestigacionFinal _editarInvestigacionFinal;
        private readonly IEditarAdminInvestigacion _editarAdminInvestigacion;
        //involucrados
        private readonly IGetInvolucradosIncidente _getInvolucradosIncidente;
        private readonly IGetInvolucradoIncidente _getInvolucradoIncidente;
        private readonly IEditarInvolucradoIncidente _editarInvolucradoIncidente;
        private readonly IEliminarInvolucradoIncidente _eliminarInvolucradoIncidente;
        private readonly IRegistrarInvolucradoIncidente _registrarInvolucradoIncidente;
        //productos
        private readonly IGetProductosDerramados _getProductosDerramados;
        private readonly IGetProductoDerramado _getProductoDerramado;
        private readonly IEditarProductoDerramado _editarProductoDerramado;
        private readonly IEliminarProductoDerramado _eliminarProductoDerramado;
        private readonly IRegistrarProductoDerramado _registrarProductoDerramado;
        //causas
        private readonly IGetCausasIncidenteInvestigacion _getCausasIncidenteInvestigacion;
        private readonly IGetCausaIncidenteInvestigacion _getCausaIncidenteInvestigacion;
        private readonly IEditarCausaIncidenteInvestigacion _editarCausaIncidenteInvestigacion;
        private readonly IEliminarCausaIncidenteInvestigacion _eliminarCausaIncidenteInvestigacion;
        private readonly IRegistrarCausaIncidenteInvestigacion _registrarCausaIncidenteInvestigacion;
        //acciones
        private readonly IGetAccioIncidenteInvestigacion _getAccioIncidenteInvestigacion;
        private readonly IGetAccionesIncidenteInvestigacion _getAccionesIncidenteInvestigacion;
        private readonly IEditarAccionIncidenteInvestigacion _editarAccionIncidenteInvestigacion;
        private readonly IEliminarAccionIncidenteInvestigacion _eliminarAccionIncidenteInvestigacion;
        private readonly IRegistrarAccionCorrectivaIncidenteInvestigacion _registrarAccionCorrectivaIncidenteInvestigacion;
        private readonly IAmpliarFechaAccion _ampliarFechaAccion;

        private readonly IEliminarEvidenciaInvestigacion _eliminarEvidenciaInvestigacion;
        private readonly IGetEvidenciaInvestigacion _getEvidenciaInvestigacion;

        private readonly IGetEvidenciasNombre _getEvidenciasNombre;

        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;

        public InvestigacionFinalController(
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IWebHostEnvironment hostingEnvironment,
            ISelectItems selectItems,
            IRegistrarInvestigacionFinal registrarInvestigacionFinal,
            IGetInvestigacionesFinales getInvestigacionesFinales,
            IGetInvestigacionFinal getInvestigacionFinal,
            IGetEstadoInvestigacion getEstadoInvestigacion,
            IGetImpactoAmbiental getImpactoAmbiental,
            IEditarInvestigacionFinal editarInvestigacionFinal,
            IEditarAdminInvestigacion editarAdminInvestigacion,
            IGetRolUsuario getRolUsuario,

            IGetInvolucradosIncidente getInvolucradosIncidente,
            IGetInvolucradoIncidente getInvolucradoIncidente,
            IEditarInvolucradoIncidente editarInvolucradoIncidente,
            IEliminarInvolucradoIncidente eliminarInvolucradoIncidente,
            IRegistrarInvolucradoIncidente registrarInvolucradoIncidente,

            IGetProductosDerramados getProductosDerramados,
            IGetProductoDerramado getProductoDerramado,
            IEditarProductoDerramado editarProductoDerramado,
            IEliminarProductoDerramado eliminarProductoDerramado,
            IRegistrarProductoDerramado registrarProductoDerramado,

            IGetCausasIncidenteInvestigacion getCausasIncidenteInvestigacion,
            IGetCausaIncidenteInvestigacion getCausaIncidenteInvestigacion,
            IEditarCausaIncidenteInvestigacion editarCausaIncidenteInvestigacion,
            IEliminarCausaIncidenteInvestigacion eliminarCausaIncidenteInvestigacion,
            IRegistrarCausaIncidenteInvestigacion registrarCausaIncidenteInvestigacion,

            IGetAccioIncidenteInvestigacion getAccioIncidenteInvestigacion,
            IGetAccionesIncidenteInvestigacion getAccionesIncidenteInvestigacion,
            IEditarAccionIncidenteInvestigacion editarAccionIncidenteInvestigacion,
            IEliminarAccionIncidenteInvestigacion eliminarAccionIncidenteInvestigacion,
            IRegistrarAccionCorrectivaIncidenteInvestigacion registrarAccionCorrectivaIncidenteInvestigacion,
            IAmpliarFechaAccion ampliarFechaAccion,

            IEliminarEvidenciaInvestigacion eliminarEvidenciaInvestigacion,
            IGetEvidenciaInvestigacion getEvidenciaInvestigacion,

            IGetEvidenciasNombre getEvidenciasNombre,

            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,

            IGetInfo_SGA info_SGA
            )
        {
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _hostingEnvironment = hostingEnvironment;
            _selectItems = selectItems;
            _registrarInvestigacionFinal = registrarInvestigacionFinal;
            _getInvestigacionesFinales = getInvestigacionesFinales;
            _getInvestigacionFinal = getInvestigacionFinal;
            _getEstadoInvestigacion = getEstadoInvestigacion;
            _getImpactoAmbiental = getImpactoAmbiental;
            _editarInvestigacionFinal = editarInvestigacionFinal;
            _editarAdminInvestigacion = editarAdminInvestigacion;

            _getInvolucradosIncidente = getInvolucradosIncidente;
            _getInvolucradoIncidente = getInvolucradoIncidente;
            _editarInvolucradoIncidente = editarInvolucradoIncidente;
            _eliminarInvolucradoIncidente = eliminarInvolucradoIncidente;
            _registrarInvolucradoIncidente = registrarInvolucradoIncidente;

            _getProductosDerramados = getProductosDerramados;
            _getProductoDerramado = getProductoDerramado;
            _editarProductoDerramado = editarProductoDerramado;
            _eliminarProductoDerramado = eliminarProductoDerramado;
            _registrarProductoDerramado = registrarProductoDerramado;

            _getCausasIncidenteInvestigacion = getCausasIncidenteInvestigacion;
            _getCausaIncidenteInvestigacion = getCausaIncidenteInvestigacion;
            _editarCausaIncidenteInvestigacion = editarCausaIncidenteInvestigacion;
            _eliminarCausaIncidenteInvestigacion = eliminarCausaIncidenteInvestigacion;
            _registrarCausaIncidenteInvestigacion = registrarCausaIncidenteInvestigacion;

            _getAccioIncidenteInvestigacion = getAccioIncidenteInvestigacion;
            _getAccionesIncidenteInvestigacion = getAccionesIncidenteInvestigacion;
            _editarAccionIncidenteInvestigacion = editarAccionIncidenteInvestigacion;
            _eliminarAccionIncidenteInvestigacion = eliminarAccionIncidenteInvestigacion;
            _registrarAccionCorrectivaIncidenteInvestigacion = registrarAccionCorrectivaIncidenteInvestigacion;
            _ampliarFechaAccion = ampliarFechaAccion;

            _eliminarEvidenciaInvestigacion = eliminarEvidenciaInvestigacion;
            _getEvidenciaInvestigacion = getEvidenciaInvestigacion;

            _getEvidenciasNombre = getEvidenciasNombre;

            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InvestigacionIdRouteValue);
            _info_SGA = info_SGA;
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
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int rpp, int pagina, string usuario_red)
        {
            var datosTabla = await _getInvestigacionesFinales.Execute(fecha_inicio, fecha_fin, gerencia, titular, estado, responsable, asesor, codigo, pagina, rpp, usuario_red);

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NInvestigaciones % rpp != 0 ? (datosTabla.NInvestigaciones / rpp) + 1 : datosTabla.NInvestigaciones / rpp;
            if (datosTabla.NInvestigaciones > 0)
            {
                foreach (var fila in datosTabla.Investigaciones)
                {
                    var IdEncriptado = _protector.Protect(fila.Id_Investigacion_Final.ToString());
                    var obsAvance = fila.NAccionesAprobadas;
                    double avance = obsAvance != 0 ? Math.Round((double)(obsAvance / fila.NAcciones * 100),1) : 0;

                    filasTabla += "<tr>";
                    if (fila.Estado.Equals(EstadosIncidentes.Pendiente.ToString()) && fila.PermisoEditar)
                        filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                    else
                        filasTabla += "<td></td>";
                    if (fila.Estado.Equals(EstadosIncidentes.Pendiente.ToString()) && fila.PermisoEditar)
                        filasTabla += "<td style=\"width: 115px;\"><a href=\"/InvestigacionFinal/EditarInvestigacion?IdInvestigacion=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    else if (datosTabla.PermisoAdmin)
                        filasTabla += "<td style=\"width: 115px;\"><a href=\"/InvestigacionFinal/EditarInvestigacion?IdInvestigacion=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    else
                        filasTabla += "<td style=\"width: 115px;\"><a href=\"/InvestigacionFinal/DetalleInvestigacion?IdInvestigacion=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.Tipo_Incidente + "</td>";
                    filasTabla += "<td>" + fila.Nivel_Incidente + "</td>";
                    filasTabla += "<td style=\"width: 150px;\">";
                    filasTabla += fila.Fecha_Incidente != null ? fila.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm") : "";
                    filasTabla += "</td>";
                    filasTabla += "<td>" + fila.Lugar_Incidente + "</td>";
                    filasTabla += "<td>" + fila.Gerencia_Responsable + "</td>";
                    filasTabla += "<td>" + fila.Supervisor_MA + "</td>";
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
                var nCol = 9;
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
        public async Task<IActionResult> EditarInvestigacion(string IdInvestigacion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdInvestigacion=" + IdInvestigacion });
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
                    x => x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Area = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Area.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Area.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Peso = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Peso.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Volumen = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Volumen.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Cantidad = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Unidad.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Cantidad.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

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

                var productos = items.ItemsProductoIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Producto_Incidente.ToString(), Text = g.Descripcion }).ToList();
                productos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposProducto = items.ItemsTipoProducto.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Producto_Incidente.ToString(), Text = g.Descripcion }).ToList();
                tiposProducto.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosEquipo = items.ItemsEstadoEquipo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Estado_Equipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
                estadosEquipo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoSuelo = items.ItemsTipoSuelo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Suelo.ToString(), Text = g.Descripcion }).ToList();
                tipoSuelo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoFlora = items.ItemsEspFlora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Especie_Flora.ToString(), Text = g.Descripcion }).ToList();
                tipoFlora.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoFauna = items.ItemsEspFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Especie_Fauna.ToString(), Text = g.Descripcion }).ToList();
                tipoFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoAgua = items.ItemsTipoAgua.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Agua.ToString(), Text = g.Descripcion }).ToList();
                tipoAgua.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugaresDisposicion = items.ItemsLugarDiposicion.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Lugar_Disposicion.ToString(), Text = g.Descripcion }).ToList();
                lugaresDisposicion.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Tipo_Incidente = tipo_Incidente;
                ViewBag.Nivel_Incidente = nivel_Incidente;
                ViewBag.Unidad_Derramado = unidad_Derramado;
                ViewBag.Unidad_Area = unidad_Area;
                ViewBag.Unidad_Peso = unidad_Peso;
                ViewBag.Unidad_Volumen = unidad_Volumen;
                ViewBag.Unidad_Cantidad = unidad_Cantidad;
                ViewBag.Componente_Ambiental = componente_Ambiental;
                ViewBag.Causa_Incidente = causa_Incidente;
                ViewBag.Gerencias = gerencias;
                ViewBag.Supervisor = supervisores;
                ViewBag.Usuario = usuario;
                ViewBag.Actos = actos;
                ViewBag.Condiciones = condiciones;
                ViewBag.FactoresPersonales = factoresPersonales;
                ViewBag.FactoresLaborales = factoresLaborales;
                ViewBag.Productos = productos;
                ViewBag.TiposProductos = tiposProducto;
                ViewBag.EstadosEquipo = estadosEquipo;

                ViewBag.TiposSuelo = tipoSuelo;
                ViewBag.TiposFlora = tipoFlora;
                ViewBag.TiposFauna = tipoFauna;
                ViewBag.TiposAgua = tipoAgua;

                ViewBag.LugaresDisposicion = lugaresDisposicion;

                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdInvestigacion));
                var investigacion = await _getInvestigacionFinal.Execute(IdDesencriptado, userInfo.NombreUsuario);

                var nombresEvidenciasJson = await _getEvidenciasNombre.Execute(IdDesencriptado);

                if (investigacion.PermisoEditar)
                {
                    var impacto = await _getImpactoAmbiental.Execute(IdDesencriptado);

                    var model = new EditarInvestigacionModel();
                    model.Id_Investigacion_Final = investigacion.Id_Investigacion_Final;
                    model.Id_Encriptado_Investigacion_Final = IdInvestigacion;
                    model.Id_Reporte_Preliminar = investigacion.Id_Reporte_Preliminar;
                    model.Codigo = investigacion.Codigo;
                    model.Num_Trabajadores = investigacion.Num_Trabajadores;
                    model.Razon_Social_Contratista = investigacion.Razon_Social_Contratista;
                    model.RUC_Contratista = investigacion.RUC_Contratista;
                    model.Actividad_Contratista = investigacion.Actividad_Contratista;
                    model.Domicilio_Contratista = investigacion.Domicilio_Contratista;
                    model.Num_Trabajadores_Contratista = investigacion.Num_Trabajadores_Contratista;
                    model.Razon_Social_SubContratista = investigacion.Razon_Social_Subcontratista;
                    model.RUC_SubContratista = investigacion.RUC_Subcontratista;
                    model.Actividad_SubContratista = investigacion.Actividad_Subcontratista;
                    model.Domicilio_SubContratista = investigacion.Domicilio_Subcontratista;
                    model.Num_Trabajadores_SubContratista = investigacion.Num_Trabajadores_Subcontratista;
                    model.Id_Tipo_Incidente = investigacion.Id_Tipo_Incidente != null ? investigacion.Id_Tipo_Incidente.Value : 0;
                    model.Id_Nivel_Incidente = investigacion.Id_Nivel_Incidente != null ? investigacion.Id_Nivel_Incidente.Value : 0;
                    model.Lugar_Incidente = investigacion.Lugar_Incidente;
                    model.Coordenada_Este = investigacion.Coordenada_Este;
                    model.Coordenada_Norte = investigacion.Coordenada_Norte;
                    model.Localidad = investigacion.Localidad;
                    model.Distrito = investigacion.Distrito;
                    model.Provincia = investigacion.Provincia;
                    model.Departamento = investigacion.Departamento;
                    model.Fecha_Incidente = investigacion.Fecha_Incidente != null ? investigacion.Fecha_Incidente.Value.ToString("yyyy-MM-dd") : "";
                    model.Hora_Incidente = investigacion.Fecha_Incidente != null ? investigacion.Fecha_Incidente.Value.ToString("HH:mm") : "";
                    model.Fecha_Inicio_Investigacion = investigacion.Fecha_Inicio_Investigacion != null ? investigacion.Fecha_Inicio_Investigacion.Value.ToString("yyyy-MM-dd") : "";
                    model.Turno = investigacion.Turno;
                    model.Descripcion_Incidente = investigacion.Descripcion_Incidente;
                    model.Id_Gerencia_Responsable = investigacion.Id_Gerencia_Responsable != null ? investigacion.Id_Gerencia_Responsable.Value : 0;
                    model.Id_Supervisor = investigacion.Id_Supervisor_MA != null ? investigacion.Id_Supervisor_MA.Value : 0;
                    model.Evidencias_Completas_Papel = investigacion.Evidencia_Papel != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Papel) : null;
                    model.Evidencias_Completas_Persona = investigacion.Evidencia_Persona != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Persona) : null;
                    model.Evidencias_Completas_Posicion = investigacion.Evidencia_Posicion != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Posicion) : null;
                    model.Evidencias_Completas_Partes = investigacion.Evidencia_Parte != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Parte) : null;
                    model.Evidencias_Nombres = nombresEvidenciasJson;
                    model.Estado = investigacion.Estado;
                    if (impacto != null)
                    {
                        model.ImpactoAmbiental = new ImpactoAmbientalModel();
                        model.ImpactoAmbiental.Id_Impacto_Ambiental_Incidente = impacto.Id_Impacto_Ambiental_Incidente;
                        model.ImpactoAmbiental.Id_Investigacion_Final = impacto.Id_Investigacion_Final;

                        //Suelo
                        model.ImpactoAmbiental.Id_Tipo_Suelo = impacto.Id_Tipo_Suelo;
                        model.ImpactoAmbiental.Area_Suelo = impacto.Area_Suelo;
                        model.ImpactoAmbiental.Id_Unidad_Area_Suelo = impacto.Id_Unidad_Area_Suelo;
                        model.ImpactoAmbiental.Cantidad_Suelo = impacto.Cantidad_Suelo;
                        model.ImpactoAmbiental.Id_Unidad_Cantidad_Suelo = impacto.Id_Unidad_Cantidad_Suelo;
                        model.ImpactoAmbiental.Id_Lugar_Disposicion_Suelo = impacto.Id_Lugar_Disposicion_Suelo;
                        //Flora
                        model.ImpactoAmbiental.Id_Especie_Flora = impacto.Id_Especie_Flora;
                        model.ImpactoAmbiental.Unidad_Flora = impacto.Unidad_Flora;
                        model.ImpactoAmbiental.Id_Unidad_Flora = impacto.Id_Unidad_Flora;
                        model.ImpactoAmbiental.Cantidad_Residuo_Flora = impacto.Cantidad_Residuo_Flora;
                        model.ImpactoAmbiental.Id_Unidad_Residuo_Flora = impacto.Id_Unidad_Residuo_Flora;
                        model.ImpactoAmbiental.Id_Lugar_Disposicion_Flora = impacto.Id_Lugar_Disposicion_Flora;
                        //Fauna
                        model.ImpactoAmbiental.Id_Especie_Fauna = impacto.Id_Especie_Fauna;
                        model.ImpactoAmbiental.Unidad_Fauna = impacto.Unidad_Fauna;
                        model.ImpactoAmbiental.Id_Unidad_Fauna = impacto.Id_Unidad_Fauna;
                        model.ImpactoAmbiental.Cantidad_Residuo_Fauna = impacto.Cantidad_Residuo_Fauna;
                        model.ImpactoAmbiental.Id_Unidad_Residuo_Fauna = impacto.Id_Unidad_Residuo_Fauna;
                        model.ImpactoAmbiental.Id_Lugar_Disposicion_Fauna = impacto.Id_Lugar_Disposicion_Fauna;
                        //Agua
                        model.ImpactoAmbiental.Id_Tipo_Agua = impacto.Id_Tipo_Agua;
                        model.ImpactoAmbiental.Volumen_Agua = impacto.Volumen_Agua;
                        model.ImpactoAmbiental.Id_Unidad_Agua = impacto.Id_Unidad_Agua;
                        model.ImpactoAmbiental.Cantidad_Residuo_Agua = impacto.Cantidad_Residuo_Agua;
                        model.ImpactoAmbiental.Id_Unidad_Residuo_Agua = impacto.Id_Unidad_Residuo_Agua;
                        model.ImpactoAmbiental.Id_Lugar_Disposicion_Agua = impacto.Id_Lugar_Disposicion_Agua;

                        model.ImpactoAmbiental.Restos_Arq = impacto.Restos_Arq;
                        model.ImpactoAmbiental.Nombre_Resto_Arq = impacto.Nombre_Resto_Arq;
                        model.ImpactoAmbiental.Cord_Este = impacto.Cord_Este;
                        model.ImpactoAmbiental.Cord_Norte = impacto.Cord_Norte;
                        model.ImpactoAmbiental.Danio_Salud = impacto.Danio_Salud;
                        model.ImpactoAmbiental.Personas_Afectadas = impacto.Personas_Afectadas;
                        model.ImpactoAmbiental.Cantidad_RS = impacto.Cantidad_RS;
                        model.ImpactoAmbiental.Id_Unidad_Cantidad_RS = impacto.Id_Unidad_Cantidad_RS;
                        model.ImpactoAmbiental.Volumen_Rs = impacto.Volumen_Rs;
                        model.ImpactoAmbiental.Id_Unidad_Volumen_RS = impacto.Id_Unidad_Volumen_RS;
                        model.ImpactoAmbiental.EO_RS = impacto.EO_RS;
                    }
                    return View(model);
                }
                else
                {
                    return RedirectToAction("DetalleInvestigacion", new { IdInvestigacion = IdInvestigacion });
                }
            }
        }
        public IActionResult DetalleInvestigacionNotificacion(int IdInvestigacion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdInvestigacion=" + IdInvestigacion });
            }
            else
            {
                return RedirectToAction("DetalleInvestigacion", new { IdInvestigacion = _protector.Protect(IdInvestigacion.ToString()) });
            }
        }
        public async Task<IActionResult> DetalleInvestigacion(string IdInvestigacion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdInvestigacion=" + IdInvestigacion });
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
                    x => x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));


                var unidad_Area = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Area.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Area.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Peso = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Peso.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Volumen = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Volumen.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Cantidad = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.Unidad.ToString()))
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Cantidad.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var componente_Ambiental = items.ItemsCompAmb.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Componente_Ambiental.ToString(), Text = g.Descripcion }).ToList();
                componente_Ambiental.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //Causa Incidente
                var grupoActo = new SelectListGroup() { Name = "Acto Subestándar", Disabled = false };
                var grupoCondicion = new SelectListGroup() { Name = "Condición Subestándar", Disabled = false };

                var causa_Incidente = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoActo }).ToList();
                causa_Incidente.AddRange(items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoCondicion }).ToList());
                causa_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
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

                var productos = items.ItemsProductoIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Producto_Incidente.ToString(), Text = g.Descripcion }).ToList();
                productos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tiposProducto = items.ItemsTipoProducto.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Producto_Incidente.ToString(), Text = g.Descripcion }).ToList();
                tiposProducto.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var estadosEquipo = items.ItemsEstadoEquipo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Estado_Equipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
                estadosEquipo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoSuelo = items.ItemsTipoSuelo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Suelo.ToString(), Text = g.Descripcion }).ToList();
                tipoSuelo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoFlora = items.ItemsEspFlora.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Especie_Flora.ToString(), Text = g.Descripcion }).ToList();
                tipoFlora.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoFauna = items.ItemsEspFauna.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Especie_Fauna.ToString(), Text = g.Descripcion }).ToList();
                tipoFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipoAgua = items.ItemsTipoAgua.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Agua.ToString(), Text = g.Descripcion }).ToList();
                tipoAgua.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var lugaresDisposicion = items.ItemsLugarDiposicion.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Lugar_Disposicion.ToString(), Text = g.Descripcion }).ToList();
                lugaresDisposicion.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Tipo_Incidente = tipo_Incidente;
                ViewBag.Nivel_Incidente = nivel_Incidente;
                ViewBag.Unidad_Derramado = unidad_Derramado;
                ViewBag.Unidad_Area = unidad_Area;
                ViewBag.Unidad_Peso = unidad_Peso;
                ViewBag.Unidad_Volumen = unidad_Volumen;
                ViewBag.Unidad_Cantidad = unidad_Cantidad;
                ViewBag.Componente_Ambiental = componente_Ambiental;
                ViewBag.Causa_Incidente = causa_Incidente;
                ViewBag.Gerencias = gerencias;
                ViewBag.Supervisor = supervisores;
                ViewBag.Usuario = usuario;
                ViewBag.Actos = actos;
                ViewBag.Condiciones = condiciones;
                ViewBag.FactoresPersonales = factoresPersonales;
                ViewBag.FactoresLaborales = factoresLaborales;
                ViewBag.Productos = productos;
                ViewBag.TiposProductos = tiposProducto;
                ViewBag.EstadosEquipo = estadosEquipo;

                ViewBag.TiposSuelo = tipoSuelo;
                ViewBag.TiposFlora = tipoFlora;
                ViewBag.TiposFauna = tipoFauna;
                ViewBag.TiposAgua = tipoAgua;

                ViewBag.LugaresDisposicion = lugaresDisposicion;

                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdInvestigacion));
                var investigacion = await _getInvestigacionFinal.Execute(IdDesencriptado, userInfo.NombreUsuario);
                var nombresEvidenciasJson = await _getEvidenciasNombre.Execute(IdDesencriptado);
                var impacto = await _getImpactoAmbiental.Execute(IdDesencriptado);

                ViewBag.PermisoAdmin = investigacion.PermisoAdmin;

                var model = new EditarInvestigacionModel();
                model.Id_Investigacion_Final = investigacion.Id_Investigacion_Final;
                model.Id_Encriptado_Investigacion_Final = IdInvestigacion;
                model.Id_Reporte_Preliminar = investigacion.Id_Reporte_Preliminar;
                model.Codigo = investigacion.Codigo;
                model.Num_Trabajadores = investigacion.Num_Trabajadores;
                model.Razon_Social_Contratista = investigacion.Razon_Social_Contratista;
                model.RUC_Contratista = investigacion.RUC_Contratista;
                model.Actividad_Contratista = investigacion.Actividad_Contratista;
                model.Domicilio_Contratista = investigacion.Domicilio_Contratista;
                model.Num_Trabajadores_Contratista = investigacion.Num_Trabajadores_Contratista;
                model.Razon_Social_SubContratista = investigacion.Razon_Social_Subcontratista;
                model.RUC_SubContratista = investigacion.RUC_Subcontratista;
                model.Actividad_SubContratista = investigacion.Actividad_Subcontratista;
                model.Domicilio_SubContratista = investigacion.Domicilio_Subcontratista;
                model.Num_Trabajadores_SubContratista = investigacion.Num_Trabajadores_Subcontratista;
                model.Id_Tipo_Incidente = investigacion.Id_Tipo_Incidente != null ? investigacion.Id_Tipo_Incidente.Value : 0;
                model.Id_Nivel_Incidente = investigacion.Id_Nivel_Incidente != null ? investigacion.Id_Nivel_Incidente.Value : 0;
                model.Lugar_Incidente = investigacion.Lugar_Incidente;
                model.Coordenada_Este = investigacion.Coordenada_Este;
                model.Coordenada_Norte = investigacion.Coordenada_Norte;
                model.Localidad = investigacion.Localidad;
                model.Distrito = investigacion.Distrito;
                model.Provincia = investigacion.Provincia;
                model.Departamento = investigacion.Departamento;
                model.Fecha_Incidente = investigacion.Fecha_Incidente != null ? investigacion.Fecha_Incidente.Value.ToString("yyyy-MM-dd") : "";
                model.Hora_Incidente = investigacion.Fecha_Incidente != null ? investigacion.Fecha_Incidente.Value.ToString("HH:mm") : "";
                model.Fecha_Inicio_Investigacion = investigacion.Fecha_Inicio_Investigacion != null ? investigacion.Fecha_Inicio_Investigacion.Value.ToString("yyyy-MM-dd") : "";
                model.Turno = investigacion.Turno;
                model.Descripcion_Incidente = investigacion.Descripcion_Incidente;
                model.Id_Gerencia_Responsable = investigacion.Id_Gerencia_Responsable != null ? investigacion.Id_Gerencia_Responsable.Value : 0;
                model.Id_Supervisor = investigacion.Id_Supervisor_MA != null ? investigacion.Id_Supervisor_MA.Value : 0;
                model.Evidencias_Completas_Papel = investigacion.Evidencia_Papel != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Papel) : null;
                model.Evidencias_Completas_Persona = investigacion.Evidencia_Persona != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Persona) : null;
                model.Evidencias_Completas_Posicion = investigacion.Evidencia_Posicion != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Posicion) : null;
                model.Evidencias_Completas_Partes = investigacion.Evidencia_Parte != null ? JsonConvert.SerializeObject(investigacion.Evidencia_Parte) : null;
                model.Evidencias_Nombres = nombresEvidenciasJson;
                model.Estado = investigacion.Estado;

                model.ImpactoAmbiental = new ImpactoAmbientalModel();
                if (impacto != null)
                {
                    model.ImpactoAmbiental.Id_Impacto_Ambiental_Incidente = impacto.Id_Impacto_Ambiental_Incidente;
                    model.ImpactoAmbiental.Id_Investigacion_Final = impacto.Id_Investigacion_Final;

                    //Suelo
                    model.ImpactoAmbiental.Id_Tipo_Suelo = impacto.Id_Tipo_Suelo;
                    model.ImpactoAmbiental.Area_Suelo = impacto.Area_Suelo;
                    model.ImpactoAmbiental.Id_Unidad_Area_Suelo = impacto.Id_Unidad_Area_Suelo;
                    model.ImpactoAmbiental.Cantidad_Suelo = impacto.Cantidad_Suelo;
                    model.ImpactoAmbiental.Id_Unidad_Cantidad_Suelo = impacto.Id_Unidad_Cantidad_Suelo;
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Suelo = impacto.Id_Lugar_Disposicion_Suelo;
                    //Flora
                    model.ImpactoAmbiental.Id_Especie_Flora = impacto.Id_Especie_Flora;
                    model.ImpactoAmbiental.Unidad_Flora = impacto.Unidad_Flora;
                    model.ImpactoAmbiental.Id_Unidad_Flora = impacto.Id_Unidad_Flora;
                    model.ImpactoAmbiental.Cantidad_Residuo_Flora = impacto.Cantidad_Residuo_Flora;
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Flora = impacto.Id_Unidad_Residuo_Flora;
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Flora = impacto.Id_Lugar_Disposicion_Flora;
                    //Fauna
                    model.ImpactoAmbiental.Id_Especie_Fauna = impacto.Id_Especie_Fauna;
                    model.ImpactoAmbiental.Unidad_Fauna = impacto.Unidad_Fauna;
                    model.ImpactoAmbiental.Id_Unidad_Fauna = impacto.Id_Unidad_Fauna;
                    model.ImpactoAmbiental.Cantidad_Residuo_Fauna = impacto.Cantidad_Residuo_Fauna;
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Fauna = impacto.Id_Unidad_Residuo_Fauna;
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Fauna = impacto.Id_Lugar_Disposicion_Fauna;
                    //Agua
                    model.ImpactoAmbiental.Id_Tipo_Agua = impacto.Id_Tipo_Agua;
                    model.ImpactoAmbiental.Volumen_Agua = impacto.Volumen_Agua;
                    model.ImpactoAmbiental.Id_Unidad_Agua = impacto.Id_Unidad_Agua;
                    model.ImpactoAmbiental.Cantidad_Residuo_Agua = impacto.Cantidad_Residuo_Agua;
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Agua = impacto.Id_Unidad_Residuo_Agua;
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Agua = impacto.Id_Lugar_Disposicion_Agua;

                    model.ImpactoAmbiental.Restos_Arq = impacto.Restos_Arq;
                    model.ImpactoAmbiental.Nombre_Resto_Arq = impacto.Nombre_Resto_Arq;
                    model.ImpactoAmbiental.Cord_Este = impacto.Cord_Este;
                    model.ImpactoAmbiental.Cord_Norte = impacto.Cord_Norte;
                    model.ImpactoAmbiental.Danio_Salud = impacto.Danio_Salud;
                    model.ImpactoAmbiental.Personas_Afectadas = impacto.Personas_Afectadas;
                    model.ImpactoAmbiental.Cantidad_RS = impacto.Cantidad_RS;
                    model.ImpactoAmbiental.Id_Unidad_Cantidad_RS = impacto.Id_Unidad_Cantidad_RS;
                    model.ImpactoAmbiental.Volumen_Rs = impacto.Volumen_Rs;
                    model.ImpactoAmbiental.Id_Unidad_Volumen_RS = impacto.Id_Unidad_Volumen_RS;
                    model.ImpactoAmbiental.EO_RS = impacto.EO_RS;
                }
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ListarInvolucrados(int IdInvestigacion)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var involucrados = await _getInvolucradosIncidente.Execute(IdInvestigacion);
            var estadoInvestigacion = await _getEstadoInvestigacion.Execute(IdInvestigacion);
            var filas_involucrados = "";
            if (involucrados.Count > 0)
            {
                foreach (var fila in involucrados)
                {
                    filas_involucrados += "<tr>";
                    filas_involucrados += "<td>" + fila.Tipo_Involucrado + "</td>";
                    filas_involucrados += "<td>" + fila.Nombres.ToUpper() + " " + fila.Apellidos.ToUpper() + "</td>";
                    if (fila.DocIdentidad != null)
                        filas_involucrados += "<td>" + fila.DocIdentidad + "</td>";
                    else
                        filas_involucrados += "<td>_</td>";
                    if (fila.Edad != null)
                        filas_involucrados += "<td>" + fila.Edad + "</td>";
                    else
                        filas_involucrados += "<td>_</td>";
                    if (fila.Puesto_Trabajo != null)
                        filas_involucrados += "<td>" + fila.Puesto_Trabajo + "</td>";
                    else
                        filas_involucrados += "<td>_</td>";
                    if (fila.Empresa != null)
                        filas_involucrados += "<td>" + fila.Empresa + "</td>";
                    else
                        filas_involucrados += "<td>_</td>";
                    if ((estadoInvestigacion.Equals(EstadosIncidentes.Pendiente.ToString()) && userInfo.Rol.Equals(InspeccionRol.AsesorMAP)) || userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        filas_involucrados += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarInvolucrado(" + fila.Id_Involucrado_Incidente + ")\">Editar</button></td>";
                        filas_involucrados += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarInvolucrado(" + fila.Id_Involucrado_Incidente + ")\">Eliminar</button></td>";
                    }
                    filas_involucrados += "</tr>";
                }
            }
            else
            {
                filas_involucrados += "<tr>";
                filas_involucrados += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
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
        public async Task<IActionResult> NuevoInvolucrado(int IdInvestigacion, string tipo, int usuario, string nombres, string apellidos, string docIdentidad, int edad, string puestoTrabajo, string empresa)
        {
            await _registrarInvolucradoIncidente.Execute(IdInvestigacion, tipo, usuario, nombres, apellidos, docIdentidad, edad, puestoTrabajo, empresa);
            return await ListarInvolucrados(IdInvestigacion);
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
                docIdentidad = involucrado.DocIdentidad,
                edad = involucrado.Edad,
                email = involucrado.Email,
                celular = involucrado.Celular,
                puesto = involucrado.Puesto_Trabajo,
                empresa = involucrado.Empresa,
                turno = involucrado.Turno,

            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarInvolucrado(int IdInvolucrado, int IdInvestigacion, string tipo, string nombres, string apellidos, int usuario, string docIdentidad, int edad, string puestoTrabajo, string empresa)
        {
            await _editarInvolucradoIncidente.Execute(IdInvolucrado, IdInvestigacion, tipo, nombres, apellidos, usuario, docIdentidad, edad, puestoTrabajo, empresa);
            return await ListarInvolucrados(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarInvolucrado(int IdInvolucrado, int IdInvestigacion)
        {
            await _eliminarInvolucradoIncidente.Execute(IdInvolucrado);
            return await ListarInvolucrados(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> ListarProductoDerramado(int IdInvestigacion)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var productos = await _getProductosDerramados.Execute(IdInvestigacion);
            var estadoInvestigacion = await _getEstadoInvestigacion.Execute(IdInvestigacion);
            var filas_productos = "";
            if (productos.Count > 0)
            {
                foreach (var fila in productos)
                {
                    filas_productos += "<tr>";
                    filas_productos += "<td>" + fila.Producto_Incidente + "</td>";
                    filas_productos += "<td>" + fila.Tipo_Producto + "</td>";
                    filas_productos += "<td>" + fila.Estado_Producto + "</td>";
                    filas_productos += "<td>" + fila.Cantidad_Derramada + " " + fila.Unidad + "</td>";
                    filas_productos += "<td>" + fila.Equipo_Responsable + "</td>";
                    filas_productos += "<td>" + fila.Estado_Equipo_Responsable + "</td>";
                    filas_productos += "<td>" + fila.Cantidad_Recuperado + " " + fila.Unidad + "</td>";
                    filas_productos += "<td>" + fila.Cantidad_No_Recuperado + " " + fila.Unidad + "</td>";
                    if ((estadoInvestigacion.Equals(EstadosIncidentes.Pendiente.ToString()) && userInfo.Rol.Equals(InspeccionRol.AsesorMAP)) || userInfo.Rol.Equals(InspeccionRol.Admin))
                    {
                        filas_productos += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarProducto(" + fila.Id_Producto_Derramado_Incidente + ")\">Editar</button></td>";
                        filas_productos += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarProducto(" + fila.Id_Producto_Derramado_Incidente + ")\">Eliminar</button></td>";
                    }
                }
            }
            else
            {
                filas_productos += "<tr>";
                filas_productos += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filas_productos += "</tr>";
            }
            dynamic response = new
            {
                filas_productos = filas_productos,
                nProductos = productos.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevoProducto(int IdInvestigacion, int producto, int tipo, string estado_producto, double cantidad_derramada, int unidad, string equipo, int estado_equipo, double cantidad_recuperado, double cantidad_no_recuperado)
        {
            await _registrarProductoDerramado.Execute(
                IdInvestigacion,
                producto,
                tipo, estado_producto,
                cantidad_derramada,
                unidad,
                equipo,
                estado_equipo,
                cantidad_recuperado,
                cantidad_no_recuperado);
            return await ListarProductoDerramado(IdInvestigacion);
        }
        [HttpGet]
        public async Task<IActionResult> EditarProducto(int IdInvolucrado)
        {
            var producto = await _getProductoDerramado.Execute(IdInvolucrado);
            dynamic response = new
            {
                idInvolucrado = producto.Id_Investigacion_Final,
                idProducto = producto.Id_Producto_Derramado_Incidente,
                producto = producto.Id_Producto_Incidente,
                tipo = producto.Id_Tipo_Producto,
                estado_producto = producto.Estado_Producto,
                cantidad_derramada = producto.Cantidad_Derramada,
                unidad = producto.Id_Unidad,
                equipo = producto.Equipo_Responsable,
                estado_equipo = producto.Id_Estado_Equipo_Responsable,
                cantidad_recuperado = producto.Cantidad_Recuperado,
                cantidad_no_recuperado = producto.Cantidad_No_Recuperado

            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarProducto(int IdProducto, int IdInvestigacion, int producto, int tipo, string estado_producto, double cantidad_derramada, int unidad, string equipo, int estado_equipo, double cantidad_recuperado, double cantidad_no_recuperado)
        {
            await _editarProductoDerramado.Execute(IdProducto, IdInvestigacion, producto, tipo, estado_producto, cantidad_derramada, unidad, equipo, estado_equipo, cantidad_recuperado, cantidad_no_recuperado);
            return await ListarProductoDerramado(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int IdProducto, int IdInvestigacion)
        {
            await _eliminarProductoDerramado.Execute(IdProducto);
            return await ListarProductoDerramado(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> ListarCausas(int IdInvestigacion)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var causas = await _getCausasIncidenteInvestigacion.Execute(IdInvestigacion);
            var estadoInvestigacion = await _getEstadoInvestigacion.Execute(IdInvestigacion);
            var filas_causas_inmediatas = "";
            var filas_causas_basicas = "";
            if (causas.Where(x => x.Tipo_Acto_Condicion.Equals("Acto Subestándar") || x.Tipo_Acto_Condicion.Equals("Condición Subestándar")).Count() == 0)
            {
                filas_causas_inmediatas += "<tr>";
                filas_causas_inmediatas += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filas_causas_inmediatas += "</tr>";
            }
            if (causas.Where(x => x.Tipo_Acto_Condicion.Equals("Factor Personal") || x.Tipo_Acto_Condicion.Equals("Factor Laboral")).Count() == 0)
            {
                filas_causas_basicas += "<tr>";
                filas_causas_basicas += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filas_causas_basicas += "</tr>";
            }
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
                    if ((estadoInvestigacion.Equals(EstadosIncidentes.Pendiente.ToString()) && userInfo.Rol.Equals(InspeccionRol.AsesorMAP)) || userInfo.Rol.Equals(InspeccionRol.Admin))
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
                    if ((estadoInvestigacion.Equals(EstadosIncidentes.Pendiente.ToString()) && userInfo.Rol.Equals(InspeccionRol.AsesorMAP)) || userInfo.Rol.Equals(InspeccionRol.Admin))
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
        public async Task<IActionResult> NuevaCausa(int IdInvestigacion, int IdActoCondicion, string Acto_Condicion, string Descripcion)
        {
            await _registrarCausaIncidenteInvestigacion.Execute(IdInvestigacion, IdActoCondicion, Acto_Condicion, Descripcion);
            return await ListarCausas(IdInvestigacion);
        }
        [HttpGet]
        public async Task<IActionResult> EditarCausa(int IdCausa)
        {
            var causa = await _getCausaIncidenteInvestigacion.Execute(IdCausa);
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
        public async Task<IActionResult> EditarCausa(int IdCausa, int IdInvestigacion, int IdActoCondicion, string Acto_Condicion, string Descripcion)
        {
            await _editarCausaIncidenteInvestigacion.Execute(IdCausa, IdInvestigacion, IdActoCondicion, Acto_Condicion, IdActoCondicion, Descripcion);
            return await ListarCausas(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarCausa(int IdCausa, int IdInvestigacion)
        {
            await _eliminarCausaIncidenteInvestigacion.Execute(IdCausa);
            return await ListarCausas(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> ListarAccionesVerificacion(int IdInvestigacion)
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

                var acciones = await _getAccionesIncidenteInvestigacion.Execute(IdInvestigacion, userInfo.NombreUsuario);

                var nVerificado = acciones.Count(x => x.Estado.Equals(EstadosIncidentes.Aprobado.ToString()));

                var estadoReporte = await _getEstadoInvestigacion.Execute(IdInvestigacion);

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
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc\" style=\"color:white;text-decoration: none;\" href=\"/ImplementarAccionMAInvestigacion/ImplementarAccionMA?IdAccion=" + IdEncriptado + "\">Cargar Evidencia" + "</a></td>";
                        else if (fila.Permiso_Implementacion && !fila.Estatus.Equals("Cerrado") && (fila.Estado.Equals(EstadosIncidentes.Registrado.ToString()) || fila.Estado.Equals(EstadosIncidentes.Rechazado.ToString()) || fila.Estado.Equals(EstadosIncidentes.Ampliado.ToString())))
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc\" style=\"color:white;text-decoration: none;\" href=\"/ImplementarAccionInvestigacion/ImplementarAccion?IdAccion=" + IdEncriptado + "\">Implementar</a></td>";
                        else if (fila.Permiso_Verificacion && fila.Estado.Equals(EstadosIncidentes.Implementado.ToString()))
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc\"style=\"color:white;text-decoration: none;\" href=\"/VerificarAccionInvestigacion/VerificarAccion?IdAccion=" + IdEncriptado + "\">Verificar</a></td>";
                        else if ((fila.Permiso_Verificacion || userInfo.Rol.Equals(InspeccionRol.Admin)) && fila.Estado.Equals(EstadosIncidentes.Vencido.ToString()))
                            filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc\" style=\"color:white;text-decoration: none;\" onclick=AmpliarFecha(" + fila.Id_Accion_Correctiva_Incidente + ")>Ampliar Fecha</button></td>";
                        else if (fila.Estado.Equals(EstadosIncidentes.Aprobado.ToString()))
                            filas_acciones += "<td><a type=\"button\" class=\"btn btn-mc-outline\" style=\"color:#2e6793;text-decoration: none;\" href=\"/VerificarAccionInvestigacion/DetalleAccion?IdAccion=" + IdEncriptado + "\">Detalle</a></td>";
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
        public async Task<IActionResult> ListarAcciones(int IdInvestigacion)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var acciones = await _getAccionesIncidenteInvestigacion.Execute(IdInvestigacion);
            var estadoReporte = await _getEstadoInvestigacion.Execute(IdInvestigacion);
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
        public async Task<IActionResult> NuevaAccion(int IdInvestigacion, string tipoAccion, string accion, int usuario, int usuario2, string contratista, string fecha, string estado)
        {
            await _registrarAccionCorrectivaIncidenteInvestigacion.Execute(IdInvestigacion, tipoAccion, accion, usuario, usuario2, contratista, fecha, estado);
            return await ListarAcciones(IdInvestigacion);
        }
        [HttpGet]
        public async Task<IActionResult> EditarAccion(int IdAccion)
        {
            var accion = await _getAccioIncidenteInvestigacion.Execute(IdAccion);

            dynamic response = new
            {
                idAccion = accion.Id_Accion_Correctiva_Incidente,
                idInvestigacion = accion.Id_Investigacion_Final,
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
        public async Task<IActionResult> EditarAccion(int IdAccion, int IdInvestigacion, string tipoAccion, int usuario, int usuario2, string accion, string contratista, string fecha, string estado)
        {
            await _editarAccionIncidenteInvestigacion.Execute(IdAccion, IdInvestigacion, tipoAccion, usuario, usuario2, accion, fecha, contratista, estado);
            return await ListarAcciones(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarAccion(int IdAccion, int IdInvestigacion)
        {
            await _eliminarAccionIncidenteInvestigacion.Execute(IdAccion);
            return await ListarAcciones(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> AmpliarFechaAccion(int IdAccion, int IdInvestigacion, string fecha)
        {
            await _ampliarFechaAccion.Execute(IdAccion, fecha);
            return await ListarAccionesVerificacion(IdInvestigacion);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarEvidencia(int IdInvestigacion, string Tipo, string Evidencia)
        {
            dynamic response;
            try
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var eliminado = await _eliminarEvidenciaInvestigacion.Execute(IdInvestigacion, Tipo, Evidencia, userInfo.NombreUsuario);
                var nombresEvidencias = await _getEvidenciasNombre.Execute(IdInvestigacion);
                var evidencia = await _getEvidenciaInvestigacion.Execute(IdInvestigacion, Tipo);
                try
                {
                    if (eliminado)
                    {
                        Tipo_Info_SGA directorio;
                        switch (Tipo.ToLower())
                        {
                            case "papel": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPapel; break;
                            case "persona": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPersona; break;
                            case "posicion": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPosicion; break;
                            default: directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPartes; break;
                        }
                        await _gestorArchivos.DeleteFTPFileAsync(Evidencia, directorio);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                    Log.Error(e.Message + " - Archivo: " + Evidencia + " - Tipo: " + Tipo);
                }
                if (eliminado)
                    response = new
                    {
                        success = true,
                        nombresEvidencias = nombresEvidencias,
                        evidencia = evidencia,
                        tipo = Tipo
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
        public async Task<IActionResult> EditarInvestigacion(EditarInvestigacionModel model)
        {
            //if (ModelState.IsValid)
            //{
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var nombresEvidencias = await _getEvidenciasNombre.Execute(model.Id_Investigacion_Final);

                var nombresArchivos = JsonConvert.DeserializeObject<List<EvidenciaCategoria>>(nombresEvidencias);

                var papeles = new List<string>();
                var papelesNombres = new List<NombreEvidencia>();
                var personas = new List<string>();
                var personasNombres = new List<NombreEvidencia>();
                var posiciones = new List<string>();
                var posicionesNombres = new List<NombreEvidencia>();
                var partes = new List<string>();
                var partesNombres = new List<NombreEvidencia>();
                var max_file_size = 8388608;
                //Papeles
                if (model.Evidencias_Completas_Papel != null)
                {
                    papeles.AddRange(new List<string>(model.Evidencias_Completas_Papel.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria papel
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("papel")) != null)
                    {
                        //Buscar evidencias de papeles
                        temp = nombresArchivos.First(x => x.Categoria.Equals("papel")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de papeles
                        temp = temp.Where(x => papeles.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria papel, agregar los existentes
                        if (papeles != null)
                        {
                            temp = papeles.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    papelesNombres.AddRange(temp);
                }
                if (model.Evidencias_Papel != null)
                {
                    for (int i = 0; i < model.Evidencias_Papel.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Papel[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Papel[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "papel") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Papel[i], newfileName, "papel");
                            papeles.Add(filePath);

                            papelesNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Papel[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Papel[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                //Personas
                if (model.Evidencias_Completas_Persona != null)
                {
                    personas.AddRange(new List<string>(model.Evidencias_Completas_Persona.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria persona
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("persona")) != null)
                    {
                        //Buscar evidencias de personas
                        temp = nombresArchivos.First(x => x.Categoria.Equals("persona")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de personas
                        temp = temp.Where(x => personas.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria persona, agregar los existentes
                        if (personas != null)
                        {
                            temp = personas.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    personasNombres.AddRange(temp);
                }
                if (model.Evidencias_Persona != null)
                {
                    for (int i = 0; i < model.Evidencias_Persona.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Persona[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Persona[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "persona") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Persona[i], newfileName, "persona");
                            personas.Add(filePath);
                            personasNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Persona[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Persona[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                //Posicion
                if (model.Evidencias_Completas_Posicion != null)
                {
                    posiciones.AddRange(new List<string>(model.Evidencias_Completas_Posicion.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria posicion
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("posicion")) != null)
                    {
                        //Buscar evidencias de posiciones
                        temp = nombresArchivos.First(x => x.Categoria.Equals("posicion")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de posiciones
                        temp = temp.Where(x => posiciones.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria posicion, agregar los existentes
                        if (posiciones != null)
                        {
                            temp = posiciones.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    posicionesNombres.AddRange(temp);
                }
                if (model.Evidencias_Posicion != null)
                {
                    for (int i = 0; i < model.Evidencias_Posicion.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Posicion[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Posicion[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "posicion") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Posicion[i], newfileName, "posicion");
                            posiciones.Add(filePath);
                            posicionesNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Posicion[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Posicion[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                //Parte
                if (model.Evidencias_Completas_Partes != null)
                {
                    partes.AddRange(new List<string>(model.Evidencias_Completas_Partes.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria parte
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("parte")) != null)
                    {
                        //Buscar evidencias de partes
                        temp = nombresArchivos.First(x => x.Categoria.Equals("parte")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de partes
                        temp = temp.Where(x => partes.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria parte, agregar los existentes
                        if (partes != null)
                        {
                            temp = partes.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    partesNombres.AddRange(temp);
                }
                if (model.Evidencias_Partes != null)
                {
                    for (int i = 0; i < model.Evidencias_Partes.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Partes[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Partes[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "parte") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Partes[i], newfileName, "parte");
                            partes.Add(filePath);
                            partesNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Partes[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Partes[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                nombresArchivos.RemoveRange(0, nombresArchivos.Count);
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "papel", Evidencias = papelesNombres });
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "persona", Evidencias = personasNombres });
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "posicion", Evidencias = posicionesNombres });
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "parte", Evidencias = partesNombres });

                await _editarInvestigacionFinal.Execute(
                    model.Id_Investigacion_Final,
                    model.Num_Trabajadores,
                    model.Razon_Social_Contratista,
                    model.RUC_Contratista,
                    model.Actividad_Contratista,
                    model.Domicilio_Contratista,
                    model.Num_Trabajadores_Contratista,
                    model.Razon_Social_SubContratista,
                    model.RUC_SubContratista,
                    model.Actividad_SubContratista,
                    model.Domicilio_SubContratista,
                    model.Num_Trabajadores_SubContratista,
                    model.Id_Tipo_Incidente,
                    model.Id_Nivel_Incidente,
                    model.Lugar_Incidente,
                    model.Coordenada_Este,
                    model.Coordenada_Norte,
                    model.Localidad,
                    model.Distrito,
                    model.Provincia,
                    model.Departamento,
                    model.Fecha_Incidente,
                    model.Hora_Incidente,
                    model.Fecha_Inicio_Investigacion,
                    model.Turno,
                    model.Descripcion_Incidente,
                    model.Id_Gerencia_Responsable,
                    model.Id_Supervisor,
                    papeles,
                    personas,
                    posiciones,
                    partes,
                    JsonConvert.SerializeObject(nombresArchivos),
                    model.ImpactoAmbiental.Id_Impacto_Ambiental_Incidente,
                    model.ImpactoAmbiental.Id_Tipo_Suelo,
                    model.ImpactoAmbiental.Area_Suelo,
                    model.ImpactoAmbiental.Id_Unidad_Area_Suelo,
                    model.ImpactoAmbiental.Cantidad_Suelo,
                    model.ImpactoAmbiental.Id_Unidad_Cantidad_Suelo,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Suelo,

                    model.ImpactoAmbiental.Id_Especie_Flora,
                    model.ImpactoAmbiental.Unidad_Flora,
                    model.ImpactoAmbiental.Id_Unidad_Flora,
                    model.ImpactoAmbiental.Cantidad_Residuo_Flora,
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Flora,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Flora,
                    model.ImpactoAmbiental.Id_Especie_Fauna,
                    model.ImpactoAmbiental.Unidad_Fauna,
                    model.ImpactoAmbiental.Id_Unidad_Fauna,
                    model.ImpactoAmbiental.Cantidad_Residuo_Fauna,
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Fauna,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Fauna,

                    model.ImpactoAmbiental.Id_Tipo_Agua,
                    model.ImpactoAmbiental.Volumen_Agua,
                    model.ImpactoAmbiental.Id_Unidad_Agua,
                    model.ImpactoAmbiental.Cantidad_Residuo_Agua,
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Agua,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Agua,
                    model.ImpactoAmbiental.Restos_Arq,
                    model.ImpactoAmbiental.Nombre_Resto_Arq,
                    model.ImpactoAmbiental.Cord_Este,
                    model.ImpactoAmbiental.Cord_Norte,
                    model.ImpactoAmbiental.Danio_Salud,
                    model.ImpactoAmbiental.Personas_Afectadas,
                    model.ImpactoAmbiental.Cantidad_RS,
                    model.ImpactoAmbiental.Id_Unidad_Cantidad_RS,
                    model.ImpactoAmbiental.Volumen_Rs,
                    model.ImpactoAmbiental.Id_Unidad_Volumen_RS,
                    model.ImpactoAmbiental.EO_RS,
                    model.Opcion
                    );
                //if (model.Opcion.Equals("Completar"))
                //    return RedirectToAction("DifundirInvestigacion", "NotificacionInvestigacionIncidente", new { IdInvestigacion = model.Id_Investigacion_Final });
                //else
                //    return View("Resumen");
            //}
            return View("Resumen");

        }
        [HttpPost]
        public async Task<IActionResult> EditarAdminInvestigacion(EditarInvestigacionModel model)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            if (userInfo.Rol.Equals(InspeccionRol.Admin))
            {
                var nombresEvidencias = await _getEvidenciasNombre.Execute(model.Id_Investigacion_Final);

                var nombresArchivos = JsonConvert.DeserializeObject<List<EvidenciaCategoria>>(nombresEvidencias);

                var papeles = new List<string>();
                var papelesNombres = new List<NombreEvidencia>();
                var personas = new List<string>();
                var personasNombres = new List<NombreEvidencia>();
                var posiciones = new List<string>();
                var posicionesNombres = new List<NombreEvidencia>();
                var partes = new List<string>();
                var partesNombres = new List<NombreEvidencia>();
                var max_file_size = 8388608;
                //Papeles
                if (model.Evidencias_Completas_Papel != null)
                {
                    papeles.AddRange(new List<string>(model.Evidencias_Completas_Papel.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria papel
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("papel")) != null)
                    {
                        //Buscar evidencias de papeles
                        temp = nombresArchivos.First(x => x.Categoria.Equals("papel")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de papeles
                        temp = temp.Where(x => papeles.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria papel, agregar los existentes
                        if (papeles != null)
                        {
                            temp = papeles.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    papelesNombres.AddRange(temp);
                }
                if (model.Evidencias_Papel != null)
                {
                    for (int i = 0; i < model.Evidencias_Papel.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Papel[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Papel[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "papel") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Papel[i], newfileName, "papel");
                            papeles.Add(filePath);

                            papelesNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Papel[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Papel[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                //Personas
                if (model.Evidencias_Completas_Persona != null)
                {
                    personas.AddRange(new List<string>(model.Evidencias_Completas_Persona.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria persona
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("persona")) != null)
                    {
                        //Buscar evidencias de personas
                        temp = nombresArchivos.First(x => x.Categoria.Equals("persona")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de personas
                        temp = temp.Where(x => personas.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria persona, agregar los existentes
                        if (personas != null)
                        {
                            temp = personas.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    personasNombres.AddRange(temp);
                }
                if (model.Evidencias_Persona != null)
                {
                    for (int i = 0; i < model.Evidencias_Persona.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Persona[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Persona[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "persona") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Persona[i], newfileName, "persona");
                            personas.Add(filePath);
                            personasNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Persona[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Persona[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                //Posicion
                if (model.Evidencias_Completas_Posicion != null)
                {
                    posiciones.AddRange(new List<string>(model.Evidencias_Completas_Posicion.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria posicion
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("posicion")) != null)
                    {
                        //Buscar evidencias de posiciones
                        temp = nombresArchivos.First(x => x.Categoria.Equals("posicion")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de posiciones
                        temp = temp.Where(x => posiciones.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria posicion, agregar los existentes
                        if (posiciones != null)
                        {
                            temp = posiciones.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    posicionesNombres.AddRange(temp);
                }
                if (model.Evidencias_Posicion != null)
                {
                    for (int i = 0; i < model.Evidencias_Posicion.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Posicion[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Posicion[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "posicion") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Posicion[i], newfileName, "posicion");
                            posiciones.Add(filePath);
                            posicionesNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Posicion[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Posicion[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                //Parte
                if (model.Evidencias_Completas_Partes != null)
                {
                    partes.AddRange(new List<string>(model.Evidencias_Completas_Partes.Split(',')));

                    var temp = new List<NombreEvidencia>();
                    //Validar nombres archivos tiene categoria parte
                    if (nombresArchivos.FirstOrDefault(x => x.Categoria.Equals("parte")) != null)
                    {
                        //Buscar evidencias de partes
                        temp = nombresArchivos.First(x => x.Categoria.Equals("parte")).Evidencias;
                        //Eliminar las evidencias que no se encuentre en la lista de partes
                        temp = temp.Where(x => partes.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene categoria parte, agregar los existentes
                        if (partes != null)
                        {
                            temp = partes.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    partesNombres.AddRange(temp);
                }
                if (model.Evidencias_Partes != null)
                {
                    for (int i = 0; i < model.Evidencias_Partes.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias_Partes[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias_Partes[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName(i + 1, "parte") + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias_Partes[i], newfileName, "parte");
                            partes.Add(filePath);
                            partesNombres.Add(new NombreEvidencia
                            {
                                NombreOrigen = model.Evidencias_Partes[i].FileName,
                                NombreCarga = filePath,
                                ExtOrigen = Path.GetExtension(model.Evidencias_Partes[i].FileName),
                                ExtCarga = Path.GetExtension(filePath)
                            });
                        }
                    }
                }

                nombresArchivos.RemoveRange(0, nombresArchivos.Count);
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "papel", Evidencias = papelesNombres });
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "persona", Evidencias = personasNombres });
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "posicion", Evidencias = posicionesNombres });
                nombresArchivos.Add(new EvidenciaCategoria { Categoria = "parte", Evidencias = partesNombres });


                await _editarAdminInvestigacion.Execute(
                    model.Id_Investigacion_Final,
                    model.Num_Trabajadores.Value,
                    model.Razon_Social_Contratista,
                    model.RUC_Contratista,
                    model.Actividad_Contratista,
                    model.Domicilio_Contratista,
                    model.Num_Trabajadores_Contratista,
                    model.Razon_Social_SubContratista,
                    model.RUC_SubContratista,
                    model.Actividad_SubContratista,
                    model.Domicilio_SubContratista,
                    model.Num_Trabajadores_SubContratista,
                    model.Id_Tipo_Incidente,
                    model.Id_Nivel_Incidente,
                    model.Lugar_Incidente,
                    model.Coordenada_Este,
                    model.Coordenada_Norte,
                    model.Localidad,
                    model.Distrito,
                    model.Provincia,
                    model.Departamento,
                    model.Fecha_Incidente,
                    model.Hora_Incidente,
                    model.Fecha_Inicio_Investigacion,
                    model.Turno,
                    model.Descripcion_Incidente,
                    model.Id_Gerencia_Responsable,
                    model.Id_Supervisor,
                    papeles,
                    personas,
                    posiciones,
                    partes,
                    JsonConvert.SerializeObject(nombresArchivos),
                    model.ImpactoAmbiental.Id_Impacto_Ambiental_Incidente,
                    model.ImpactoAmbiental.Id_Tipo_Suelo,
                    model.ImpactoAmbiental.Area_Suelo,
                    model.ImpactoAmbiental.Id_Unidad_Area_Suelo,
                    model.ImpactoAmbiental.Cantidad_Suelo,
                    model.ImpactoAmbiental.Id_Unidad_Cantidad_Suelo,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Suelo,

                    model.ImpactoAmbiental.Id_Especie_Flora,
                    model.ImpactoAmbiental.Unidad_Flora,
                    model.ImpactoAmbiental.Id_Unidad_Flora,
                    model.ImpactoAmbiental.Cantidad_Residuo_Flora,
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Flora,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Flora,
                    model.ImpactoAmbiental.Id_Especie_Fauna,
                    model.ImpactoAmbiental.Unidad_Fauna,
                    model.ImpactoAmbiental.Id_Unidad_Fauna,
                    model.ImpactoAmbiental.Cantidad_Residuo_Fauna,
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Fauna,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Fauna,

                    model.ImpactoAmbiental.Id_Tipo_Agua,
                    model.ImpactoAmbiental.Volumen_Agua,
                    model.ImpactoAmbiental.Id_Unidad_Agua,
                    model.ImpactoAmbiental.Cantidad_Residuo_Agua,
                    model.ImpactoAmbiental.Id_Unidad_Residuo_Agua,
                    model.ImpactoAmbiental.Id_Lugar_Disposicion_Agua,
                    model.ImpactoAmbiental.Restos_Arq,
                    model.ImpactoAmbiental.Nombre_Resto_Arq,
                    model.ImpactoAmbiental.Cord_Este,
                    model.ImpactoAmbiental.Cord_Norte,
                    model.ImpactoAmbiental.Danio_Salud,
                    model.ImpactoAmbiental.Personas_Afectadas,
                    model.ImpactoAmbiental.Cantidad_RS,
                    model.ImpactoAmbiental.Id_Unidad_Cantidad_RS,
                    model.ImpactoAmbiental.Volumen_Rs,
                    model.ImpactoAmbiental.Id_Unidad_Volumen_RS,
                    model.ImpactoAmbiental.EO_RS
                    );
            }
            return RedirectToAction("DetalleInvestigacion", new { IdInvestigacion = _protector.Protect(model.Id_Investigacion_Final.ToString()) });
        }
        [HttpPost]
        public async Task<IActionResult> ObtenerImagenesCargadas(string imagenes)
        {
            dynamic response;
            if (HttpContext.Session.GetString("SessionUser") != null && imagenes != null)
            {
                var listaImagenes = new List<string>(imagenes.Split(','));
                var archivos = JsonConvert.DeserializeObject<List<EvidenciaCategoria>>(imagenes);
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var imagesSrc = new List<ImagenCargada>();

                for (int i = 0; i < archivos.Count; i++)
                {
                    foreach (var evidencia in archivos[i].Evidencias)
                    {
                        try
                        {
                            var encryptedFile = "";
                            if (archivos[i].Categoria == "papel")
                                encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(evidencia.NombreCarga, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPapel);
                            else if (archivos[i].Categoria == "persona")
                                encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(evidencia.NombreCarga, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPersona);
                            else if (archivos[i].Categoria == "posicion")
                                encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(evidencia.NombreCarga, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPosicion);
                            else if (archivos[i].Categoria == "parte")
                                encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(evidencia.NombreCarga, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPartes);
                            /* Save encrypted file in temp path*/
                            var tempPath = Path.GetTempPath();

                            var SGATempPath = tempPath + "Descarga Temporal Gestiona\\";
                            if (!Directory.Exists(SGATempPath))
                            {
                                Directory.CreateDirectory(SGATempPath);
                            }
                            var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                            var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                            /*decrypt file*/
                            var decryptedFileName = "desencriptado" + i.ToString() + Path.GetExtension(evidencia.NombreCarga);
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
                            var newFileName = evidencia.NombreCarga.Replace(Path.GetExtension(evidencia.NombreCarga), Path.GetExtension(filePath));
                            /*read decrypted file*/
                            //byte[] imageByteData = System.IO.File.ReadAllBytes(archivePath + decryptedFileName);
                            string imageBase64Data = Convert.ToBase64String(bytes);
                            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);

                            imagesSrc.Add(new ImagenCargada
                            {
                                Nombre = evidencia.NombreOrigen,
                                NombreCarga = evidencia.NombreCarga,
                                tipo = archivos[i].Categoria,
                                Src = imageDataURL,
                                EsImagen = (evidencia.ExtOrigen.ToLower() == ".jpg" || evidencia.ExtOrigen.ToLower() == ".jpeg" || evidencia.ExtOrigen.ToLower() == ".png" || evidencia.ExtCarga.ToLower() == ".jpg" || evidencia.ExtCarga.ToLower() == ".jpeg" || evidencia.ExtCarga.ToLower() == ".png")
                            });
                        }
                        catch (Exception ex)
                        {
                            //No se encontró el archivo 
                        }
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
        //Carga y descarga de archivos
        private string CreateUniqueFileName(int index, string tipo)
        {
            switch (tipo)
            {
                case "papel": return "Ev_Inc_Inv_Papel_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + index;
                case "persona": return "Ev_Inc_Inv_Persona_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + index;
                case "posicion": return "Ev_Inc_Inv_Posicion_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + index;
                default: return "Ev_Inc_Inv_Parte_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + index;
            }
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
        public async Task<IActionResult> Download(string fileName, string tipo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?fileName=" + fileName + "&tipo=" + tipo });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                try
                {
                    Tipo_Info_SGA directorio;
                    switch (tipo)
                    {
                        case "papel": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPapel; break;
                        case "persona": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPersona; break;
                        case "posicion": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPosicion; break;
                        default: directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPartes; break;
                    }
                    var encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(fileName, userInfo.NombreUsuario, directorio);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "Descarga Temporal Gestiona\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = archivePath + "desencriptado.zip";
                    await _gestorEncriptado.DecryptFileAsync(encryptedFile, decryptedFileName);
                    //4. descomprimir en carpeta temporal submission
                    if (!filePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                        filePath += Path.DirectorySeparatorChar;

                    using (ZipArchive archive = ZipFile.OpenRead(decryptedFileName))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            filePath = Path.GetFullPath(Path.Combine(filePath, entry.FullName));
                            if (filePath.StartsWith(filePath, StringComparison.Ordinal))
                                entry.ExtractToFile(filePath);
                        }
                    }
                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                    var newFileName = fileName.Replace(Path.GetExtension(fileName), Path.GetExtension(filePath));
                    return File(bytes, GetContentType(filePath), newFileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }
        public async Task<string> UploadZipFileAsync(IFormFile file, string newFileName, string tipo)
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
                var SGATempPath = tempPath + "Descarga Temporal Gestiona/";
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
                Tipo_Info_SGA directorio;
                switch (tipo)
                {
                    case "papel": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPapel; break;
                    case "persona": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPersona; break;
                    case "posicion": directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPosicion; break;
                    default: directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncInvPartes; break;
                }
                return await _gestorArchivos.UploadFTPFileAsync(encryptedArchiveFile, newFileName, directorio);
            }
        }        
        private class EvidenciaCategoria
        {
            public string Categoria { get; set; }
            public List<NombreEvidencia> Evidencias { get; set; }
        }
        private class ImagenCargada
        {
            public string Nombre { get; set; }
            public string Src { get; set; }
            public string NombreCarga { get; set; }
            public string tipo { get; set; }
            public bool EsImagen { get; set; }
        }
    }
}