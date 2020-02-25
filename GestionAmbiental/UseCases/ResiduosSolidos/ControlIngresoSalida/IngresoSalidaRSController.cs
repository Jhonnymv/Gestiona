using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ConsultarIngresoSalidaRSs;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.RegistrarIngresoSalidaRS;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.ResiduosSolidos.ControlIngresoSalida
{
    public class IngresoSalidaRSController : Controller
    {
        private readonly IGetSelectItems _getSelectItems;
        private readonly IConsultarIngresoSalidaRSs _consultarIngresoSalidaRSs;
        private readonly IRegistrarIngresoSalidaRS _registrarIngresoSalidaRS;

        public IngresoSalidaRSController(IGetSelectItems getSelectItems, IConsultarIngresoSalidaRSs consultarIngresoSalidaRSs, IRegistrarIngresoSalidaRS registrarIngresoSalidaRS)
        {
            _getSelectItems = getSelectItems;
            _consultarIngresoSalidaRSs = consultarIngresoSalidaRSs;
            _registrarIngresoSalidaRS = registrarIngresoSalidaRS;
        }

        public async Task<IActionResult> Resumen()
        {
            var items = await _getSelectItems.Execute();
            var acr_cmrs = items.ItemsACR_CMRS.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_ACR_CMRS.ToString(), Text = g.Descripcion }).ToList();
            acr_cmrs.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            var ingreso_salida = items.ItemsIngresoSalida.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Ingreso_Salida.ToString(), Text = g.Descripcion }).ToList();
            ingreso_salida.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            var tipo_residuo = items.ItemsTipoResiduo.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Residuo.ToString(), Text = g.Descripcion }).ToList();
            tipo_residuo.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            var empresa = items.ItemsEmpresa.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
            empresa.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            var asesoresMA = items.ItemsAsesoresMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.ACR_CMRS = acr_cmrs;
            ViewBag.IngresoSalida = ingreso_salida;
            ViewBag.TipoResiduo = tipo_residuo;
            ViewBag.Empresa = empresa;
            ViewBag.AsesoresMA = asesoresMA;
            return View();
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(int acr_cmrs, int ingreso_salida, int tipo, string fecha, int empresa, int asesorMA, int pagina, int rpp)
        {
            var datosTabla = await _consultarIngresoSalidaRSs.Execute(acr_cmrs, ingreso_salida, tipo, fecha, empresa, asesorMA, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Registros)
                {
                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"#" + "\">"+fila.Item + "</a></td>";
                    filasTabla += "<td>" + fila.ACR_CMRS + "</td>";
                    filasTabla += "<td>" + fila.Responsable_ACR_CMRS + "</td>";
                    filasTabla += "<td>" + fila.Ingreso_Salida + "</td>";
                    filasTabla += "<td>" + fila.Motivo + "</td>";
                    filasTabla += "<td>" + fila.Fecha_Ingreso_Salida.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.Tipo_Residuo + "</td>";
                    filasTabla += "<td>" + fila.Residuo_Solido + "</td>";
                    filasTabla += "<td>" + fila.Cantidad + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    //filasTabla += "<td><div class=\"progress\"><div class=\"progress-bar bg-info\" role=\"progressbar\" style=\"width:" + avance + "%; color: black;font-weight: bold;\" aria-valuemin=\"0\" aria-valuemax=\"100\">" + avance + "%</div></div></td>";
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
        public async Task<IActionResult> NuevoRegistro()
        {
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

            ViewBag.ACR_CMRS = acr_cmrs;
            ViewBag.IngresoSalida = ingreso_salida;
            ViewBag.Motivo = motivo;
            ViewBag.Empresa = empresa;
            ViewBag.TiposResiduo = tipos_residuos;
            ViewBag.ResiduosSolidosPelig = residuos_solidos_p;
            ViewBag.ResiduosSolidosNoPelig = residuos_solidos_np;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(IngresoSalidaRSModel model)
        {
            if (ModelState.IsValid)
            {
                var IdIngresoSalida = await _registrarIngresoSalidaRS.Execute(model.ACR_CMRS, model.Ingreso_Salida, model.Motivo, model.Residuo_Solido, model.Fecha, model.Responsable_ACR_CMRS, model.Cantidad, model.Empresa);
                return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoRegistro");
        }
    }
}