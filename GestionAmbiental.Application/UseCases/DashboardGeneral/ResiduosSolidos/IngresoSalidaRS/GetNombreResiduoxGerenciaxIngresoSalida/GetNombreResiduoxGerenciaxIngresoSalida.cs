using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxGerenciaxIngresoSalida
{
    public class GetNombreResiduoxGerenciaxIngresoSalida : IGetNombreResiduoxGerenciaxIngresoSalida
    {
        private readonly IGetDataIngresoSalidaResiduosDashboardReadOnlyRepository _getDataIngresoSalidaResiduosDashboardReadOnlyRepository;

        public GetNombreResiduoxGerenciaxIngresoSalida(IGetDataIngresoSalidaResiduosDashboardReadOnlyRepository getDataIngresoSalidaResiduosDashboardReadOnlyRepository)
        {
            _getDataIngresoSalidaResiduosDashboardReadOnlyRepository = getDataIngresoSalidaResiduosDashboardReadOnlyRepository;
        }

        public async Task<List<IngresoSalidaNombreResiduo>> Execute(int top,int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int[] residuos, int[] gerencias)
        {
            DateTime FechaInicio, FechaFin;

            if (fechaInicio != null)
                FechaInicio = Convert.ToDateTime(fechaInicio);
            else
                FechaInicio = DateTime.MinValue;

            if (fechaFin != null)
                FechaFin = Convert.ToDateTime(fechaFin);
            else
                FechaFin = DateTime.MaxValue;

            var output =  await _getDataIngresoSalidaResiduosDashboardReadOnlyRepository.GetNombreResiduoxGerencia(
                acr_cmrs,
                ingreso_salida,
                tipo_residuo,
                motivo,
                FechaInicio,
                FechaFin,
                residuos,
                gerencias);
            //output = output.OrderByDescending(x=>x.TotalIngreso).Take(top).ToList();
            return output;
        }
    }
}