using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientosFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.Items;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.AvistamientoFauna
{
    public class AvistamientoFaunaController : Controller
    {
        private readonly IGetItems _getItems;
        private readonly IGetAvistamientosFauna _getAvistamientosFauna;
        private readonly IDataProtector _protector;
        public AvistamientoFaunaController(
            IGetItems getItems, 
            IGetAvistamientosFauna getAvistamientosFauna,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getItems = getItems;
            _getAvistamientosFauna = getAvistamientosFauna;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.AvistamientoFaunaIdRouteValue);
        }

        public async Task<IActionResult> Resumen()
        {
            var items = await _getItems.Execute();
            
            var tiposFauna = items.TiposFauna.Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
            tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var estadosFauna = items.EstadosFauna.Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
            estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.TipoFauna = tiposFauna;
            ViewBag.EstadoFauna = estadosFauna;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha, int tipo_fauna, int estado_fauna, int rpp, int pagina)
        {
            var datosTabla = await _getAvistamientosFauna.Execute(fecha,tipo_fauna,estado_fauna,rpp,pagina);

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NAvistamientos % rpp != 0 ? (datosTabla.NAvistamientos / rpp) + 1 : datosTabla.NAvistamientos / rpp;
            if (datosTabla.NAvistamientos > 0)
            {
                foreach (var fila in datosTabla.Avistamientos)
                {
                    //var IdEncriptado = _protector.Protect(fila.Id_Investigacion_Final.ToString());
                    
                    filasTabla += "<tr>";
                    if (fila.Estado.Equals(EstadosAvistamiento.Registrado.ToString()))
                        filasTabla += "<td><img class=\"icon\" src=\"../img/icons/bell.png\"/></td>";
                    else
                        filasTabla += "<td></td>";

                    var IdEncriptado = _protector.Protect(fila.IdAvistamientoFauna.ToString());
                    filasTabla += "<td><a href=\"/AvistamientoFauna/EditarAvistamiento?IdAvistamiento=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.Fecha_Avistamiento.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.Fecha_Avistamiento.ToString("HH:mm") + "</td>";
                    filasTabla += "<td>" + fila.Ubicacion + "</td>";
                    filasTabla += "<td>" + fila.Cantidad + "</td>";
                    filasTabla += "<td>" + fila.TipoFauna + "</td>"; 
                    filasTabla += "<td>" + fila.NombreComun + "</td>"; 
                    filasTabla += "<td>" + fila.EstadoFauna + "</td>"; 
                    filasTabla += "<td>" + fila.TipoAvistamiento + "</td>"; 
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
        public async Task<IActionResult> CatalogoFauna()
        {
            return View();
        }
        public async Task<IActionResult> CargarCatalogo()
        {
            return View();
        }
        public async Task<IActionResult> NuevoRegistro()
        {
            var items = await _getItems.Execute();

            var tiposFauna = items.TiposFauna.Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
            tiposFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var estadosFauna = items.EstadosFauna.Select(g => new SelectListItem() { Value = g.IdEstadoFauna.ToString(), Text = g.Descripcion }).ToList();
            estadosFauna.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.TipoFauna = tiposFauna;
            ViewBag.EstadoFauna = estadosFauna;

            return View();
        }
    }
}