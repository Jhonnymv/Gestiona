using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetDestinoxNombreResiudoxIngresoSalida;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxIngresoSalida;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GestionAmbiental.UseCases.DashboardGeneral
{
    public class DashboardResiduosController : Controller
    {
        //Ingreso y Salida
        private readonly IGetDestinoxNombreResiduoxIngresoSalida _getDestinoxNombreResiduoxIngresoSalida;
        private readonly IGetNombreResiduoxIngresoSalida _getNombreResiduoxIngresoSalida;

        public DashboardResiduosController(
            IGetDestinoxNombreResiduoxIngresoSalida getDestinoxNombreResiduoxIngresoSalida, 
            IGetNombreResiduoxIngresoSalida getNombreResiduoxIngresoSalida)
        {
            _getDestinoxNombreResiduoxIngresoSalida = getDestinoxNombreResiduoxIngresoSalida;
            _getNombreResiduoxIngresoSalida = getNombreResiduoxIngresoSalida;
        }
        [HttpGet]
        public async Task<IActionResult> GetIngresoSalidaTipoResiduo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa)
        {
            var registros = await _getNombreResiduoxIngresoSalida.Execute(acr_cmrs,ingreso_salida,tipo_residuo,motivo, fechaInicio, fechaFin, gerencia,empresa);
            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetDestinoxTipoResiduoxSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa)
        {
            var registros = await _getDestinoxNombreResiduoxIngresoSalida.Execute(acr_cmrs,ingreso_salida,tipo_residuo,motivo, fechaInicio, fechaFin, gerencia,empresa);
            var registrosFiltrados = new List<Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida.DestinoResiduos>();
            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }
    }
}