using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestosRSCierre;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.ResiduosSolidos.ControlManifiestoRS.CompletarManifiestoRS
{
    public class CompletarManifiestoController : Controller
    {
        private readonly IGetSelectItems _getSelectItems;
        private readonly IConsultarManifiestosRSCierre _consultarManifiestosRS;
        private readonly IDataProtector _protector;

        public CompletarManifiestoController(
            IGetSelectItems getSelectItems,
            IConsultarManifiestosRSCierre consultarManifiestosRS,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getSelectItems = getSelectItems;
            _consultarManifiestosRS = consultarManifiestosRS;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.IngresoResiduosPeligrosSIdRouteValue);
        }

        public async Task<IActionResult> Resumen()
        {
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
        public async Task<IActionResult> GetTablaResumenFiltrada(string nManifiesto, string fechaEmision, string observacion, int residuoSolido, int asesorMAP, int eors_salida, int eors_llegada, int pagina, int rpp)
        {
            var datosTabla = await _consultarManifiestosRS.Execute(nManifiesto, fechaEmision, observacion, residuoSolido, eors_salida, eors_llegada, asesorMAP, pagina, rpp);
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
                    switch (fila.Observacion_Manifiesto)
                    {
                        case "1": filasTabla += "<td>" + "Conforme" + "</td>"; break;
                        case "2": filasTabla += "<td>" + "Anulado" + "</td>"; break;
                    }
                    filasTabla += "<td>" + fila.Residuo_Solido + "</td>";
                    filasTabla += "<td>" + fila.Cantidad_RS_Evacuado + "</td>";
                    filasTabla += "<td>" + fila.EO_RS_Salida + "</td>";
                    filasTabla += "<td>" + fila.EO_RS_Llegada + "</td>";
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

}