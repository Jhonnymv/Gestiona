using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes.Inspecciones;
using GestionAmbiental.Domain.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.Reportes.Inspecciones
{
    public class ReportesInspeccionesController : Controller
    {
        private readonly IGetObservacionesMAPvsSSO _getInspeccionesMAPvsSSO;
        private readonly IGetObservacionesXUsuario _getObservacionesXUsuario;
        private readonly IGetObservacionesXEstado _getObservacionesXEstado;
        private readonly IGetResumenObservacionesXUsuario _getResumenObservacionesXUsuario;
        private readonly IGetInspeccionesDiarias _getInspeccionesDiarias;
        private readonly IGetObservacionesXNivelRiesgoMAP _getObservacionesXNivelRiesgoMAP;
        private readonly IGetObservacionesXNivelRiesgoSSO _getObservacionesXNivelRiesgoSSO;
        private readonly IGetObservacionesXActoCondicionMAP _getObservacionesXActoCondicionMAP;
        private readonly IGetObservacionesXActoCondicionSSO _getObservacionesXActoCondicionSSO;

        public ReportesInspeccionesController(IGetObservacionesMAPvsSSO getInspeccionesMAPvsSSO, IGetObservacionesXUsuario getObservacionesXUsuario, IGetObservacionesXEstado getObservacionesXEstado, IGetResumenObservacionesXUsuario getResumenObservacionesXUsuario, IGetInspeccionesDiarias getInspeccionesDiarias, IGetObservacionesXNivelRiesgoMAP getObservacionesXNivelRiesgoMAP, IGetObservacionesXNivelRiesgoSSO getObservacionesXNivelRiesgoSSO, IGetObservacionesXActoCondicionMAP getObservacionesXActoCondicionMAP, IGetObservacionesXActoCondicionSSO getObservacionesXActoCondicionSSO)
        {
            _getInspeccionesMAPvsSSO = getInspeccionesMAPvsSSO;
            _getObservacionesXUsuario = getObservacionesXUsuario;
            _getObservacionesXEstado = getObservacionesXEstado;
            _getResumenObservacionesXUsuario = getResumenObservacionesXUsuario;
            _getInspeccionesDiarias = getInspeccionesDiarias;
            _getObservacionesXNivelRiesgoMAP = getObservacionesXNivelRiesgoMAP;
            _getObservacionesXNivelRiesgoSSO = getObservacionesXNivelRiesgoSSO;
            _getObservacionesXActoCondicionMAP = getObservacionesXActoCondicionMAP;
            _getObservacionesXActoCondicionSSO = getObservacionesXActoCondicionSSO;
        }

        public async Task<IActionResult> Resumen()
        {
            var nObservaciones = await _getObservacionesXEstado.Execute();
            var inspecciones_diarias = await _getInspeccionesDiarias.Execute();
            var map_vs_sso = await _getInspeccionesMAPvsSSO.Execute();
            var MAP_x_nivel_riesgo = await _getObservacionesXNivelRiesgoMAP.Execute();
            var SSO_x_nivel_riesgo = await _getObservacionesXNivelRiesgoSSO.Execute();
            var MAP_x_tipo_observacion = await _getObservacionesXActoCondicionMAP.Execute();
            var SSO_x_tipo_observacion = await _getObservacionesXActoCondicionSSO.Execute();


            ViewBag.nObservaciones = nObservaciones.Select(x => x.Valor).ToList();


            var model = new List<ReportesModel>();
            model.Add(new ReportesModel("Cantidad de inspecciones de los últimos 30 días", inspecciones_diarias));
            model.Add(new ReportesModel("Cantidad de Observaciones MAP vs SSO", map_vs_sso));
            model.Add(new ReportesModel("Cantidad de Observaciones MAP por Nivel de Riesgo", MAP_x_nivel_riesgo));
            model.Add(new ReportesModel("Cantidad de Observaciones SSO por Nivel de Riesgo", SSO_x_nivel_riesgo));
            model.Add(new ReportesModel("Cantidad de Observaciones MAP por Tipo de Observación", MAP_x_tipo_observacion));
            model.Add(new ReportesModel("Cantidad de Observaciones SSO por Tipo de Observación", SSO_x_tipo_observacion));

            return View(model);
        }
        /*public async Task<IActionResult> MAPvsSSO()
        {
            var data = await _getInspeccionesMAPvsSSO.Execute();
            var model = new ReportesModel();

            model.titulo = "Observaciones MAP vs SSO";
            model.data = data; 

            return View(model);
        }
        public async Task<IActionResult> ObservacionesXUsuario()
        {
            var data = await _getObservacionesXUsuario.Execute();
            var model = new ReportesModel();
            model.titulo = "Observaciones por Usuario";
            model.data = data;

            return View(model);
        }
        public async Task<IActionResult> ObservacionesXEstado()
        {
            var data = await _getObservacionesXEstado.Execute();
            var model = new ReportesModel();
            model.titulo = "Estado de Observaciones";
            model.data = data;

            return View(model);
        }
        public async Task<IActionResult> DetalleObservacionesXUsuario()
        {
            var data = await _getResumenObservacionesXUsuario.Execute("");
            ViewBag.DataTable = data;
            return View();
        }*/
    }
}