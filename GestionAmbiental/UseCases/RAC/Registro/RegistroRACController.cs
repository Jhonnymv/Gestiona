using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.RAC.Registro;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.RAC.Registro
{
    public class RegistroRACController : Controller
    {
        private readonly IConsultarRACs _consultarRACs;

        public RegistroRACController(IConsultarRACs consultarRACs)
        {
            _consultarRACs = consultarRACs ?? throw new ArgumentNullException(nameof(consultarRACs));
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
    }
}