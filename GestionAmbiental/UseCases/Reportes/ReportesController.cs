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

        public ReportesController(IGetObservacionesMAPvsSSO getInspeccionesMAPvsSSO, IGetObservacionesXUsuario getObservacionesXUsuario)
        {
            _getInspeccionesMAPvsSSO = getInspeccionesMAPvsSSO;
            _getObservacionesXUsuario = getObservacionesXUsuario;
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
    }
}