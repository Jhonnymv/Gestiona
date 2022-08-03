using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxDestinoxIngresoSalida;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxGerenciaxIngresoSalida;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxIngresoSalida;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetTopGerenciaIngreso;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GestionAmbiental.UseCases.DashboardGeneral
{
    public class DashboardResiduosController : Controller
    {
        //Ingreso y Salida
        private readonly IGetNombreResiduoxDestinoxIngresoSalida _getNombreResiduoxDestinoxIngresoSalida;
        private readonly IGetNombreResiduoxIngresoSalida _getNombreResiduoxIngresoSalida;
        private readonly IGetTopGerenciaIngreso _getTopGerenciaIngreso;
        private readonly IGetNombreResiduoxGerenciaxIngresoSalida _getNombreResiduoxGerenciaxIngresoSalida;

        public DashboardResiduosController(
            IGetNombreResiduoxDestinoxIngresoSalida getNombreResiduoxDestinoxIngresoSalida, 
            IGetNombreResiduoxIngresoSalida getNombreResiduoxIngresoSalida,
            IGetTopGerenciaIngreso getTopGerenciaIngreso,
            IGetNombreResiduoxGerenciaxIngresoSalida getNombreResiduoxGerenciaxIngresoSalida)
        {
            _getNombreResiduoxDestinoxIngresoSalida = getNombreResiduoxDestinoxIngresoSalida;
            _getNombreResiduoxIngresoSalida = getNombreResiduoxIngresoSalida;
            _getTopGerenciaIngreso = getTopGerenciaIngreso;
            _getNombreResiduoxGerenciaxIngresoSalida = getNombreResiduoxGerenciaxIngresoSalida;
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
        public async Task<IActionResult> GetTipoResiduoxDestinoxSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int[] residuos, int[] destinos)
        {
            var registros = await _getNombreResiduoxDestinoxIngresoSalida.Execute(acr_cmrs,ingreso_salida,tipo_residuo,motivo, fechaInicio, fechaFin, gerencia,residuos,destinos);
            registros = registros.Where(x=>x.TotalSalida>0).ToList();
            registros.ForEach(x=>x.Destinos = x.Destinos.Where(x=>x.TotalSalida>0).ToList());
            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }      
        
        [HttpGet]
        public async Task<IActionResult> GetTopGerenciasIngreso(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int empresa)
        {
            var registros = await _getTopGerenciaIngreso.Execute(10,acr_cmrs,ingreso_salida,tipo_residuo,motivo, fechaInicio, fechaFin, gerencia,empresa);
            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }    
        [HttpGet]
        public async Task<IActionResult> GetTopGerenciasIngresoTipoResiduo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int gerencia, int[] residuos, int[] gerencias)
        {
            var registros = await _getNombreResiduoxGerenciaxIngresoSalida.Execute(10,acr_cmrs,ingreso_salida,tipo_residuo,motivo, fechaInicio, fechaFin, residuos, gerencias);
            registros.ForEach(x=>x.Gerencias = x.Gerencias.OrderByDescending(y=>y.Ingreso).ToList());
            dynamic response = new
            {
                registros = registros
            };
            return Ok(response);
        }  
    }
}