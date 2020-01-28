using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetItemsFiltroResumen;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetNReportesEntreFechas;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetObservacionesXEstado;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.Reportes.RAC
{
    public class ReportesRACController : Controller
    {
        private readonly IGetObservacionesXEstado _getObservacionesXEstado;
        private readonly IGetItemsFiltroResumen _getItemsFiltroResumen;
        private readonly IGetNReportesEntreFechas _getNReportesEntreFechas;
         
        public ReportesRACController(IGetObservacionesXEstado getObservacionesXEstado, IGetItemsFiltroResumen getItemsFiltroResumen,IGetNReportesEntreFechas getNReportesEntreFechas)
        {
            _getObservacionesXEstado = getObservacionesXEstado;
            _getItemsFiltroResumen = getItemsFiltroResumen;
            _getNReportesEntreFechas = getNReportesEntreFechas;
        }

        public async Task<IActionResult> Resumen()
        {
            var model = new FiltroReportesRACModel();

            var selectItemsResumen = await _getItemsFiltroResumen.Execute();

            var filtroGerencias = selectItemsResumen.Gerencias.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
            filtroGerencias.Insert(0, new SelectListItem("Todo", "0", true));

            var filtroEmpresas = selectItemsResumen.Empresas.OrderBy(x => x.RazonSocial).Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
            filtroEmpresas.Insert(0, new SelectListItem("Todo", "0", true));

            var filtroUsuarios = selectItemsResumen.Usuarios.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres+" "+g.Apellidos }).ToList();
            filtroUsuarios.Insert(0, new SelectListItem("Todo", "0", true));

            ViewBag.ItemsGerencia = filtroGerencias;
            ViewBag.ItemsEmpresa = filtroEmpresas;
            ViewBag.ItemsUsuario = filtroUsuarios;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Filtrar(int IdGerencia,int IdEmpresa, int IdUsuario)
        {
            //Observaciones x estado filtradas y ordenadas en funcion al Enum EstadosRAC
            var nObservaciones = await _getObservacionesXEstado.Execute(IdGerencia, IdEmpresa, IdUsuario);

            dynamic response = new
            {
                nObservaciones = nObservaciones,
                fInicio = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"),
                fFin = DateTime.Now.ToString("yyyy-MM-dd")
            };
            return Ok(response);
        }
        public async Task<IActionResult> GetNReportesEntreFechas(int IdGerencia, int IdEmpresa, int IdUsuario,string FechaInicio,string FechaFin)
        {
            var nReportesEntreFechas = await _getNReportesEntreFechas.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
            dynamic response = new
            {
                nReportesEntreFechas = nReportesEntreFechas
            };
            return Ok(response);
        }
    }
}