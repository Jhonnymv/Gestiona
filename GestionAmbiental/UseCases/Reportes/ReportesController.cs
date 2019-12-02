using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Reportes;
using GestionAmbiental.Domain.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.Reportes
{
    public class ReportesController : Controller
    {
        private readonly IGetObservacionesMAPvsSSO _getInspeccionesMAPvsSSO;
        private readonly IGetObservacionesXUsuario _getObservacionesXUsuario;
        private readonly IGetObservacionesXEstado _getObservacionesXEstado;
        private readonly IGetResumenObservacionesXUsuario _getResumenObservacionesXUsuario;

        public ReportesController(IGetObservacionesMAPvsSSO getInspeccionesMAPvsSSO, IGetObservacionesXUsuario getObservacionesXUsuario, IGetObservacionesXEstado getObservacionesXEstado, IGetResumenObservacionesXUsuario getResumenObservacionesXUsuario)
        {
            _getInspeccionesMAPvsSSO = getInspeccionesMAPvsSSO;
            _getObservacionesXUsuario = getObservacionesXUsuario;
            _getObservacionesXEstado = getObservacionesXEstado;
            _getResumenObservacionesXUsuario = getResumenObservacionesXUsuario;
        }

        public async Task<IActionResult> MAPvsSSO()
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
        }
    }
}