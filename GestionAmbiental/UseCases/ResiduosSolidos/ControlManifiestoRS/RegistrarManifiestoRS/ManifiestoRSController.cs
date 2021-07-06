using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestosRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EditarManifiestoRS.EditarManifiestoRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.RegistrarManifiestoRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.ResiduosSolidos.ControlManifiestoRS.RegistrarManifiestoRS
{
    public class ManifiestoRSController : Controller
    {
        private readonly IGetSelectItems _getSelectItems;
        private readonly IConsultarManifiestosRS _consultarManifiestosRS;
        private readonly IRegistrarManifiestoRS _registrarManifiestoRS;
        private readonly IConsultarManifiestoRS _consultarManifiestoRS;
        private readonly IEditarManifiestoRS _editarManifiestoRS;
        private readonly IDataProtector _protector;

        public ManifiestoRSController(
            IGetSelectItems getSelectItems,
            IConsultarManifiestosRS consultarManifiestosRS,
            IRegistrarManifiestoRS registrarManifiestoRS,
            IConsultarManifiestoRS consultarManifiestoRS,
            IEditarManifiestoRS editarManifiestoRS,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings
            )
        {
            _getSelectItems = getSelectItems;
            _consultarManifiestosRS = consultarManifiestosRS;
            _consultarManifiestoRS = consultarManifiestoRS;
            _registrarManifiestoRS = registrarManifiestoRS;
            _editarManifiestoRS = editarManifiestoRS;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.IngresoResiduosPeligrosSIdRouteValue);
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

                var items = await _getSelectItems.Execute();
                var residuos_solidos = items.ItemsResiduoSolido.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var eo_rs = items.ItemsEO_RS.OrderBy(x => x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_EO_RS.ToString(), Text = g.Razon_Social }).ToList();
                eo_rs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var asesoresMA = items.ItemsAsesoresMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.ResiduosSolidos = residuos_solidos;
                ViewBag.EO_RS = eo_rs;
                ViewBag.AsesoresMA = asesoresMA;
                return View();
            }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string nManifiesto, string fechaEmision, string observacion, int residuoSolido, int eors_salida, int eors_llegada, int pagina, int rpp)
        {
            var datosTabla = await _consultarManifiestosRS.Execute(nManifiesto, fechaEmision, observacion, residuoSolido, eors_salida, eors_llegada,0, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.nManifiesto % rpp != 0 ? (datosTabla.nManifiesto / rpp) + 1 : datosTabla.nManifiesto / rpp;
            if (datosTabla.nManifiesto > 0)
            {
                foreach (var fila in datosTabla.manifiestosRS)
                {
                    var id_encriptado = _protector.Protect(fila.Id_Manifiesto_RS.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/ManifiestoRS/EditarManifiesto?IdManifiesto=" + id_encriptado + "\">" + fila.Item + "</a></td>";
                    filasTabla += "<td>" + fila.Serie_Manifiesto + "</td>";
                    filasTabla += "<td>" + fila.Num_Manifiesto + "</td>";
                    filasTabla += "<td>" + fila.Fecha_Emision_Manifiesto.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.Observacion_Manifiesto + "</td>";
                    filasTabla += "<td>" + fila.Residuo_Solido + "</td>";
                    filasTabla += "<td>" + fila.Cantidad_RS_Evacuado + "</td>";
                    filasTabla += "<td>" + fila.EO_RS_Salida + "</td>";
                    filasTabla += "<td>" + fila.EO_RS_Llegada + "</td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina-2; i <= pagina+2; i++)
                {
                    if (i > 0 && i<=nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }                   
                }
                if (pagina <= nPaginas-4 && nPaginas > 5)
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
        public async Task<IActionResult> NuevoManifiesto()
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

                var items = await _getSelectItems.Execute();
                var residuos_solidos = items.ItemsResiduoSolido.Where(x => x.Tipo_Residuo.Id_Tipo_Residuo.Equals(1)).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var eo_rs = items.ItemsEO_RS.OrderBy(x => x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_EO_RS.ToString(), Text = g.Razon_Social }).ToList();
                eo_rs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var conductores = items.ItemsConductor.Where(x=>x.Estado.Equals("ACT")).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Conductor_RS.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                conductores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var vehiculos = items.ItemsVehiculo.OrderBy(x => x.Placa).Select(g => new SelectListItem() { Value = g.Id_Vehiculo_RS.ToString(), Text = g.Placa }).ToList();
                vehiculos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //Complementos de vehiculo (Para completar automáticamente los demás campos)
                var compVehiculos = items.ItemsVehiculo.Select(x => new { Id_Vehiculo_RS = x.Id_Vehiculo_RS, Capacidad = x.Capacidad }).ToList();
                ViewBag.ComVehiculos = JsonConvert.SerializeObject(compVehiculos);
                //Complementos de conductor (Para completar automáticamente los demás campos)
                var compConductores = items.ItemsConductor.Where(x => x.Estado.Equals("ACT")).Select(x => new { Id_Conductor_RS = x.Id_Conductor_RS, NLicencia = x.NLicencia }).ToList();
                ViewBag.CompConductores = JsonConvert.SerializeObject(compConductores);
                //Complementos de EO-RS (Para completar automáticamente los demás campos)
                var compEO_RS = items.ItemsEO_RS.Select(x => new { Id_EO_RS = x.Id_EO_RS, Registro = x.Registro, Fecha_Vencimiento_Registro = x.Fecha_Vencimiento_Registro.ToString("yyyy-MM-dd") }).ToList();
                ViewBag.CompEORS = JsonConvert.SerializeObject(compEO_RS);

                ViewBag.Conductores = conductores;
                ViewBag.Vehiculos = vehiculos;
                ViewBag.ResiduosSolidos = residuos_solidos;
                ViewBag.EO_RS = eo_rs;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoManifiesto(ManifiestoRSModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var IdManifiesto = await _registrarManifiestoRS.Execute(
                    model.opcion,
                    model.Serie_Manifiesto,
                    model.Num_Manifiesto,
                    model.Fecha_Emision_Manifiesto,
                    model.Observacion_Manifiesto,
                    model.Id_Residuo_Solido,
                    model.Cantidad_RS_Evacuado,
                    model.Id_EO_RS_Salida,
                    model.Registro_EO_RS_Salida,
                    model.Fecha_Vencimiento_Registro_Salida,
                    model.Num_Autorizacion_Municipal_Salida,
                    model.Num_Autorizacion_Hoja_Ruta,
                    model.Id_Conductor,
                    model.Num_Licencia_Condutor,
                    model.Id_Vehiculo,
                    model.Capacidad_Vehiculo,
                    model.Id_EO_RS_Llegada,
                    model.Registro_EO_RS_Llegada,
                    model.Fecha_Vencimiento_Registro_Llegada,
                    model.Num_Autorizacion_Sanitaria_Llegada,
                    model.Num_Autorizacion_Municipal_Llegada,
                    userInfo.NombreUsuario);
                if (model.opcion.Equals("Registrar") && model.Observacion_Manifiesto.Equals("Conforme"))
                    return RedirectToAction("NotificarRegistroRS", "NotificacionesRS", new { Id_Manifiesto = IdManifiesto });
                else
                    return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoManifiesto");
        }
        public async Task<IActionResult> EditarManifiesto(string IdManifiesto)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdManifiesto=" + IdManifiesto});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var IdDecrypted = Convert.ToInt32(_protector.Unprotect(IdManifiesto));
                var data = await _consultarManifiestoRS.Execute(IdDecrypted, userInfo.NombreUsuario);
                var model = new ManifiestoRSModel();
                model.Id_Encriptado = IdManifiesto;
                model.Id_Manifiesto_RS = IdDecrypted;
                model.Serie_Manifiesto = data.Serie_Manifiesto;
                model.Num_Manifiesto = data.Num_Manifiesto;
                model.Fecha_Emision_Manifiesto = data.Fecha_Emision_Manifiesto.ToString("yyyy-MM-dd");
                model.Observacion_Manifiesto = data.Observacion_Manifiesto;
                model.Id_Residuo_Solido = data.Id_Resido_Solido;
                model.Cantidad_RS_Evacuado = data.Cantidad_RS_Evacuado;
                model.Id_EO_RS_Salida = data.Id_EO_RS_Salida;
                model.Registro_EO_RS_Salida = data.Registro_EO_RS_Salida;
                model.Fecha_Vencimiento_Registro_Salida = data.Fecha_Vencimiento_Registro_Salida != null ? data.Fecha_Vencimiento_Registro_Salida.Value.ToString("yyyy-MM-dd") : "";
                model.Num_Autorizacion_Municipal_Salida = data.Num_Autorizacion_Municipal_Salida;
                model.Num_Autorizacion_Hoja_Ruta = data.Num_Autorizacion_Hoja_Ruta;
                //Conductor
                model.Id_Conductor = data.Id_Conductor;
                model.Num_Licencia_Condutor = data.Num_Licencia_Condutor;
                model.Id_Vehiculo = data.Id_Vehiculo;
                model.Capacidad_Vehiculo = data.Capacidad_Vehiculo;
                model.Id_EO_RS_Llegada = data.Id_EO_RS_Llegada;
                model.Registro_EO_RS_Llegada = data.Registro_EO_RS_Llegada;
                model.Fecha_Vencimiento_Registro_Llegada = data.Fecha_Vencimiento_Registro_Llegada!=null?data.Fecha_Vencimiento_Registro_Llegada.Value.ToString("yyyy-MM-dd") : "";
                model.Num_Autorizacion_Sanitaria_Llegada = data.Num_Autorizacion_Sanitaria_Llegada;
                model.Num_Autorizacion_Municipal_Llegada = data.Num_Autorizacion_Municipal_Llegada;
                model.Estado = data.Estado;
                model.Permiso = data.Permiso;

                var items = await _getSelectItems.Execute();
                var residuos_solidos = items.ItemsResiduoSolido.Where(x => x.Tipo_Residuo.Id_Tipo_Residuo.Equals(1)).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var eo_rs = items.ItemsEO_RS.OrderBy(x => x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_EO_RS.ToString(), Text = g.Razon_Social }).ToList();
                eo_rs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var conductores = items.ItemsConductor.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Conductor_RS.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                conductores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var vehiculos = items.ItemsVehiculo.OrderBy(x => x.Placa).Select(g => new SelectListItem() { Value = g.Id_Vehiculo_RS.ToString(), Text = g.Placa }).ToList();
                vehiculos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Conductores = conductores;
                ViewBag.Vehiculos = vehiculos;
                ViewBag.ResiduosSolidos = residuos_solidos;
                ViewBag.EO_RS = eo_rs;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarManifiesto(ManifiestoRSModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                await _editarManifiestoRS.Execute(
                    model.Id_Manifiesto_RS,
                    model.opcion,
                    model.Serie_Manifiesto,
                    model.Num_Manifiesto,
                    model.Fecha_Emision_Manifiesto,
                    model.Observacion_Manifiesto,
                    model.Id_Residuo_Solido,
                    model.Cantidad_RS_Evacuado,
                    model.Id_EO_RS_Salida,
                    model.Registro_EO_RS_Salida,
                    model.Fecha_Vencimiento_Registro_Salida,
                    model.Num_Autorizacion_Municipal_Salida,
                    model.Num_Autorizacion_Hoja_Ruta,
                    model.Id_Conductor,
                    model.Num_Licencia_Condutor,
                    model.Id_Vehiculo,
                    model.Capacidad_Vehiculo,
                    model.Id_EO_RS_Llegada,
                    model.Registro_EO_RS_Llegada,
                    model.Fecha_Vencimiento_Registro_Llegada,
                    model.Num_Autorizacion_Sanitaria_Llegada,
                    model.Num_Autorizacion_Municipal_Llegada,
                    userInfo.NombreUsuario);
                if (model.opcion.Equals("Registrar"))
                    return RedirectToAction("NotificarRegistroRS", "NotificacionesRS", new { Id_Manifiesto = model.Id_Manifiesto_RS });
                else
                    return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoManifiesto");
        }
    }
}