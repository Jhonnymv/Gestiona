using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRSs;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRSs;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.EditarIngresoSalidaRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.EliminarRegistroIngresoSalidaRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.RegistrarIngresoSalidaRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.EditarIngresoSalidaRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.EliminarRegistroIngresoSalidaRS;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.RegistrarIngresoSalidaRS;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace GestionAmbiental.UseCases.ResiduosSolidos.ControlIngresoSalida
{
    public class IngresoSalidaRSController : Controller
    {
        private readonly IGetSelectItems _getSelectItems;
        private readonly IConsultarIngresoSalidaRSs _consultarIngresoSalidaRSs;
        private readonly IRegistrarIngresoSalidaRS _registrarIngresoSalidaRS;
        private readonly IConsultarIngresoSalidaRS _consultarIngresoSalidaRS;
        private readonly IEditarIngresoSalidaRS _editarIngresoSalidaRS;
        private readonly IEliminarRegistroIngresoSalidaRS _eliminarRegistroIngresoSalidaRS;

        private readonly IDataProtector _protector;
        public IngresoSalidaRSController(
            IGetSelectItems getSelectItems,
            IConsultarIngresoSalidaRSs consultarIngresoSalidaRSs,
            IRegistrarIngresoSalidaRS registrarIngresoSalidaRS,
            IConsultarIngresoSalidaRS consultarIngresoSalidaRS,
            IEditarIngresoSalidaRS editarIngresoSalidaRS,
            IEliminarRegistroIngresoSalidaRS eliminarRegistroIngresoSalidaRS,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getSelectItems = getSelectItems;
            _consultarIngresoSalidaRSs = consultarIngresoSalidaRSs;
            _registrarIngresoSalidaRS = registrarIngresoSalidaRS;
            _consultarIngresoSalidaRS = consultarIngresoSalidaRS;
            _editarIngresoSalidaRS = editarIngresoSalidaRS;
            _eliminarRegistroIngresoSalidaRS = eliminarRegistroIngresoSalidaRS;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.IngresoSalidaRSIdRouteValue);
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

                var items = await _getSelectItems.Execute();
                var acr_cmrs = items.ItemsACR_CMRS.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_ACR_CMRS.ToString(), Text = g.Descripcion }).ToList();
                acr_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var ingreso_salida = items.ItemsIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Ingreso_Salida.ToString(), Text = g.Descripcion }).ToList();
                ingreso_salida.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var tipo_residuo = items.ItemsTipoResiduo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Residuo.ToString(), Text = g.Descripcion }).ToList();
                tipo_residuo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var empresa = items.ItemsEmpresa.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresa.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var responsable_arc_cmrs = items.ItemsResponsableACR_CMRS.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Responsable_ACR_CRMS.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsable_arc_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                
                ViewBag.ACR_CMRS = acr_cmrs;
                ViewBag.IngresoSalida = ingreso_salida;
                ViewBag.TipoResiduo = tipo_residuo;
                ViewBag.Empresa = empresa;
                ViewBag.Gerencias = gerencias;
                return View();
            }
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(int acr_cmrs, int ingreso_salida, int tipo, string fecha, int empresa, int gerencia, int pagina, int rpp)
        {
            var datosTabla = await _consultarIngresoSalidaRSs.Execute(acr_cmrs, ingreso_salida, tipo, fecha, empresa, gerencia, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Registros)
                {
                    var id_encriptado = _protector.Protect(fila.Id_Ingreso_Salida_RS.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/IngresoSalidaRS/EditarRegistro?IdRegistro="+ id_encriptado + "\">"+fila.Item + "</a></td>";
                    filasTabla += "<td>" + fila.ACR_CMRS + "</td>";
                    filasTabla += "<td>" + fila.Responsable_ACR_CMRS + "</td>";
                    filasTabla += "<td>" + fila.Ingreso_Salida + "</td>";
                    filasTabla += "<td>" + fila.Motivo + "</td>";
                    filasTabla += "<td>" + fila.Fecha_Ingreso_Salida.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.Tipo_Residuo + "</td>";
                    filasTabla += "<td>" + fila.Residuo_Solido + "</td>";
                    filasTabla += "<td>" + fila.Cantidad + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
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
        public async Task<IActionResult> NuevoRegistro()
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

                var items = await _getSelectItems.Execute();
                var acr_cmrs = items.ItemsACR_CMRS.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_ACR_CMRS.ToString(), Text = g.Descripcion }).ToList();
                acr_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var ingreso_salida = items.ItemsIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Ingreso_Salida.ToString(), Text = g.Descripcion }).ToList();
                ingreso_salida.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var motivo = items.ItemsMotivoIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Motivo.ToString(), Text = g.Descripcion }).ToList();
                motivo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var empresa = items.ItemsEmpresa.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresa.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var tipos_residuos = items.ItemsTipoResiduo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Residuo.ToString(), Text = g.Descripcion }).ToList();
                tipos_residuos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var residuos_solidos_p = items.ItemsResiduoSolido.Where(x => x.Tipo_Residuo.Id_Tipo_Residuo.Equals((int)TipoResiduoSolido.Peligroso)).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos_p.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var residuos_solidos_np = items.ItemsResiduoSolido.Where(x => x.Tipo_Residuo.Id_Tipo_Residuo.Equals((int)TipoResiduoSolido.NoPeligroso)).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos_np.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var responsable_arc_cmrs = items.ItemsResponsableACR_CMRS.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Responsable_ACR_CRMS.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsable_arc_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion}).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.ACR_CMRS = acr_cmrs;
                ViewBag.IngresoSalida = ingreso_salida;
                ViewBag.Motivo = motivo;
                ViewBag.Empresa = empresa;
                ViewBag.TiposResiduo = tipos_residuos;
                ViewBag.ResiduosSolidosPelig = residuos_solidos_p;
                ViewBag.ResiduosSolidosNoPelig = residuos_solidos_np;
                ViewBag.ResponsablesARC_CMRS = responsable_arc_cmrs;
                ViewBag.Gerencias = gerencias;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(IngresoSalidaRSModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                
                var IdIngresoSalida = await _registrarIngresoSalidaRS.Execute(model.opcion,model.ACR_CMRS, model.Ingreso_Salida, model.Motivo, model.Residuo_Solido, model.Fecha, model.Responsable_ACR_CMRS, model.Cantidad, model.Empresa,model.Gerencia,model.Comentario,userInfo.NombreUsuario);
                return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoRegistro");
        }
        public async Task<IActionResult> EditarRegistro(string IdRegistro)
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

                var id_decrypted = Convert.ToInt32(_protector.Unprotect(IdRegistro));

                var data = await _consultarIngresoSalidaRS.Execute(id_decrypted, userInfo.NombreUsuario);

                var items = await _getSelectItems.Execute();
                var acr_cmrs = items.ItemsACR_CMRS.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_ACR_CMRS.ToString(), Text = g.Descripcion }).ToList();
                acr_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var ingreso_salida = items.ItemsIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Ingreso_Salida.ToString(), Text = g.Descripcion }).ToList();
                ingreso_salida.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var motivo = items.ItemsMotivoIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Motivo.ToString(), Text = g.Descripcion }).ToList();
                motivo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var empresa = items.ItemsEmpresa.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresa.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var tipos_residuos = items.ItemsTipoResiduo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Residuo.ToString(), Text = g.Descripcion }).ToList();
                tipos_residuos.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var residuos_solidos_p = items.ItemsResiduoSolido.Where(x => x.Tipo_Residuo.Id_Tipo_Residuo.Equals((int)TipoResiduoSolido.Peligroso)).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos_p.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var residuos_solidos_np = items.ItemsResiduoSolido.Where(x => x.Tipo_Residuo.Id_Tipo_Residuo.Equals((int)TipoResiduoSolido.NoPeligroso)).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Residuo_Solido.ToString(), Text = g.Descripcion }).ToList();
                residuos_solidos_np.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var responsable_arc_cmrs = items.ItemsResponsableACR_CMRS.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Responsable_ACR_CRMS.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
                responsable_arc_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.ACR_CMRS = acr_cmrs;
                ViewBag.IngresoSalida = ingreso_salida;
                ViewBag.Motivo = motivo;
                ViewBag.Empresa = empresa;
                ViewBag.TiposResiduo = tipos_residuos;
                ViewBag.ResiduosSolidosPelig = residuos_solidos_p;
                ViewBag.ResiduosSolidosNoPelig = residuos_solidos_np;
                ViewBag.ResponsablesARC_CMRS = responsable_arc_cmrs;
                ViewBag.Gerencias = gerencias;

                var model = new IngresoSalidaRSModel();
                model.Encrypted_Id = IdRegistro;
                model.Id_Ingreso_Salida_RS = id_decrypted;
                model.ACR_CMRS = data.Id_ACR_CMRS;
                model.Ingreso_Salida = data.Id_Ingreso_Salida;
                model.Motivo = data.Id_Motivo;
                model.Residuo_Solido = data.Id_Residuo_Solido;
                model.Tipo_Residuo = data.Id_Tipo_Residuo;
                model.Responsable_ACR_CMRS = data.Id_Responsable_ACR_CMRS;
                model.Comentario = data.Comentario;
                model.Empresa = data.Id_Empresa;
                model.Fecha = data.Fecha_Ingreso_Salida.ToString("yyyy-MM-dd");
                model.Gerencia = data.Id_Gerencia;
                model.Cantidad = data.Cantidad;
                model.Residuo_Solido = data.Id_Residuo_Solido;
                model.Estado = data.Estado;
                model.Permiso = data.Permiso;
                model.PermisoAdmin = data.PermisoAdmin;
                model.PermisoMA = data.PermisoMA;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarRegistro(IngresoSalidaRSModel model)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                
                await _editarIngresoSalidaRS.Execute(model.Id_Ingreso_Salida_RS,model.opcion, model.ACR_CMRS, model.Ingreso_Salida, model.Motivo, model.Residuo_Solido, model.Fecha, model.Responsable_ACR_CMRS, model.Cantidad, model.Empresa, model.Gerencia, model.Comentario, userInfo.NombreUsuario);
                return RedirectToAction("Resumen");
            }
            var IdEncrypted = _protector.Protect(model.Id_Ingreso_Salida_RS.ToString());
            return RedirectToAction("EditarRegistro",new { IdRegistro = IdEncrypted });
        }
        public async Task<IActionResult> EliminarRegistro(string IdRegistro)
        {
            if (ModelState.IsValid)
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                var decrypted_id = Convert.ToInt32(_protector.Unprotect(IdRegistro));
                await _eliminarRegistroIngresoSalidaRS.Execute(decrypted_id, userInfo.NombreUsuario);
               
                return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoRegistro");
        }
    }
}