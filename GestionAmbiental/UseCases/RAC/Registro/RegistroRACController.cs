using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.RAC.Registro;
using GestionAmbiental.Application.UseCases.RAC.Registro.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.RAC.Registro
{
    public class RegistroRACController : Controller
    {
        private readonly IConsultarRACs _consultarRACs;
        private readonly ISelectItems _selectItems;
        public RegistroRACController(IConsultarRACs consultarRACs, ISelectItems selectItems)
        {
            _consultarRACs = consultarRACs ?? throw new ArgumentNullException(nameof(consultarRACs));
            _selectItems = selectItems ?? throw new ArgumentNullException(nameof(selectItems));
        }
        public IActionResult Resumen()
        {
            return View();
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, string filtroCodigo, int pagina, int rpp)
        {

            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarRACs.Execute(pagina, rpp, fecha, filtroCodigo);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Registros)
                {
                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"#\">" + fila.Codigo + "</a></td>";
                    filasTabla += "<td>" + fila.Nombres_Apellidos + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.Empresa_Ob + "</td>";
                    filasTabla += "<td>" + fila.Gerencia_Obs + "</td>";
                    filasTabla += "<td>" + fila.SSO_MA + "</td>";
                    filasTabla += "<td>" + fila.Clasificacion + "</td>";
                    filasTabla += "<td>" + fila.NivelRiesgo + "</td>";
                    filasTabla += "<td>" + fila.TipoCondicion + "</td>";
                    filasTabla += "<td>" + fila.Fecha.ToString("dd/MM/yyyy HH:mm") + "</td>";
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
        public async Task<IActionResult> NuevoRAC()
        {
            var items = await _selectItems.Execute();

            var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
            gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var empresas = items.ItemsEmpresa.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
            empresas.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposActoMA = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
            tiposActoMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposCondicionMA = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
            tiposCondicionMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposActoSSO = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
            tiposActoSSO.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposCondicionSSO = items.ItemsTiposCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion }).ToList();
            tiposCondicionSSO.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var usuarios = items.ItemsUsuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres+" "+g.Apellidos }).ToList();
            usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var asesoresMAP = items.ItemsAsesoresMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            asesoresMAP.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var asesoresSSO = items.ItemsAsesoresSSO.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            asesoresSSO.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Gerencias = gerencias;
            ViewBag.Empresas = empresas;
            ViewBag.Usuarios = usuarios;
            ViewBag.AsesoresMAP = asesoresMAP;
            ViewBag.AsesoresSSO = asesoresSSO;

            ViewBag.ActoMA = tiposActoMA;
            ViewBag.CondicionMA = tiposCondicionMA;
            ViewBag.ActoSSO = tiposActoSSO;
            ViewBag.CondicionSSO = tiposCondicionSSO;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRAC(NuevoRACModel model)
        {
            return View();
        }
    }
}