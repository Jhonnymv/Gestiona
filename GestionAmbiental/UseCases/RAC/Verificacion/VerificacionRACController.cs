using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.RAC.Registro.SelectItems;
using GestionAmbiental.Application.UseCases.RAC.Verificacion.AprobarObservacion;
using GestionAmbiental.Application.UseCases.RAC.Verificacion.ConsultarRAC;
using GestionAmbiental.Application.UseCases.RAC.Verificacion.ConsultarRACs;
using GestionAmbiental.Application.UseCases.RAC.Verificacion.RechazarObservacion;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.RAC.Verificacion
{
    public class VerificacionRACController : Controller
    {
        private readonly ISelectItems _selectItems;
        private readonly IConsultarRAC _consultarRAC;
        private readonly IConsultarRACs _consultarRACs;
        private readonly IAprobarObservacion _aprobarObservacion;
        private readonly IRechazarObservacion _rechazarObservacion;

        public VerificacionRACController(
            ISelectItems selectItems,
            IConsultarRAC consultarRAC,
            IConsultarRACs consultarRACs,
            IAprobarObservacion aprobarObservacion,
            IRechazarObservacion rechazarObservacion
            )
        {
            _selectItems = selectItems;
            _consultarRAC = consultarRAC;
            _consultarRACs = consultarRACs;
            _aprobarObservacion = aprobarObservacion;
            _rechazarObservacion = rechazarObservacion;
        }

        public async Task<IActionResult> Resumen()
        {
            var items = await _selectItems.Execute();
            var usuarios = items.ItemsUsuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            ViewBag.Usuarios = usuarios;
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
                    filasTabla += "<td><label style=\"width:80px;\"><a href=\"/VerificacionRAC/VerificarRAC?IdRAC=" + fila.Id_RAC + "\">" + fila.Codigo + "</a></label></td>";
                    filasTabla += "<td>" + fila.Nombres_Apellidos + "</td>";
                    filasTabla += "<td>" + fila.Empresa + "</td>";
                    filasTabla += "<td>" + fila.Empresa_Ob + "</td>";
                    filasTabla += "<td>" + fila.Gerencia_Obs + "</td>";
                    filasTabla += "<td>" + fila.SSO_MA + "</td>";
                    filasTabla += "<td>" + fila.Clasificacion + "</td>";
                    switch (fila.NivelRiesgo)
                    {
                        case "Tolerable": filasTabla += "<td><span class=\"badge badge-success\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Alerta": filasTabla += "<td><span class=\"badge badge-warning\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "No Aceptable": filasTabla += "<td><span class=\"badge badge-danger\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Bajo": filasTabla += "<td><span class=\"badge badge-success\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Medio": filasTabla += "<td><span class=\"badge badge-warning\">" + fila.NivelRiesgo + "</span></td>"; break;
                        case "Alto": filasTabla += "<td><span class=\"badge badge-danger\">" + fila.NivelRiesgo + "</span></td>"; break;
                        default: filasTabla += "<td><span class=\"badge badge-secondary\">" + fila.NivelRiesgo + "</span>"; break;
                    }
                    filasTabla += "<td>" + fila.TipoCondicion + "</td>";
                    filasTabla += "<td>" + fila.Supervisor + "</td>";
                    filasTabla += "<td>" + fila.Fecha.ToString("dd/MM/yyyy HH:mm") + "</td>";
                    if (EstadosRAC.Rechazado.ToString() == fila.Estado)
                    {
                        filasTabla += "<td>Pendiente de implementación</td>";
                    }
                    else if (EstadosRAC.Implementado.ToString() == fila.Estado)
                    {
                        filasTabla += "<td>Pendiente de verificación</td>";
                    }
                    else
                    {
                        filasTabla += "<td>Cerrado</td>";
                    }
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

        public async Task<IActionResult> VerificarRAC(int IdRAC)
        {
            var reporte = await _consultarRAC.Execute(IdRAC,User.Identity.Name);
            var model = new VerificarRACModel
            {
                Id_RAC = reporte.Id_RAC,
                Nombres_Apellidos = reporte.Nombres_Apellidos,
                Gerencia = reporte.Gerencia,
                Empresa = reporte.Empresa,
                Otra_Empresa = reporte.Otra_Empresa,
                Empresa_Obs = reporte.Empresa_Ob,
                Gerencia_Obs = reporte.Gerencia_Obs,
                Ubicacion = reporte.Ubicacion,
                Fecha = reporte.Fecha.ToString("dd/MM/yyyy"),
                Hora = reporte.Fecha.ToString("HH:mm"),
                SSO_MA = reporte.SSO_MA,
                Clasificacion = reporte.Clasificacion,
                NivelRiesgo = reporte.NivelRiesgo,
                Tipo_Condicion = reporte.TipoCondicion,
                Descripcion_Acto_Cond = reporte.Descripcion_Acto_Cond,
                Medida_Control = reporte.Medida_Control,
                PAC = reporte.PAC,
                Responsable_Cierre = reporte.Responsable_Cierre,
                Fecha_Cierre = reporte.Fecha_Cierre.ToString("dd/MM/yyyy"),
                Supervisor = reporte.Supervisor,
                Evidencia_Obs = reporte.Evidencia_Obs,
                Evidencia_Cierre = reporte.Evidencia_Cierre,
                Comentario_Cierre = reporte.Comentario_Cierre,
                Comentario_Verificacion = reporte.Comentario_Verificacion,
                Estado = reporte.Estado
            };
            ViewBag.Permiso = reporte.Permiso;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> VerificarRAC(VerificarRACModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Opcion.Equals("Aprobar"))
                    await _aprobarObservacion.Execute(model.Id_RAC, model.Comentario_Verificacion, User.Identity.Name);
                else
                    await _rechazarObservacion.Execute(model.Id_RAC, model.Comentario_Verificacion, User.Identity.Name);
            }
            return RedirectToAction("VerificarRAC", new { IdRAC = model.Id_RAC });
        }
    }
}