using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetDestinoxNombreResiudoxIngresoSalida
{
    public class GetDestinoxNombreResiduoxIngresoSalida : IGetDestinoxNombreResiduoxIngresoSalida
    {
        private readonly GetDataIngresoSalidaResiduosDashboardReadOnlyRepository _getDataIngresoSalidaResiduosDashboardReadOnlyRepository;
        public GetDestinoxNombreResiduoxIngresoSalida(GetDataIngresoSalidaResiduosDashboardReadOnlyRepository getDataIngresoSalidaResiduosDashboardReadOnlyRepository)
        {
            _getDataIngresoSalidaResiduosDashboardReadOnlyRepository = getDataIngresoSalidaResiduosDashboardReadOnlyRepository;
        }
        public async Task<List<DestinoResiduos>> Execute(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int empresa)
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

            return await _getDataIngresoSalidaResiduosDashboardReadOnlyRepository.GetDestinoxNombreResiduoxIngresoSalida(
                acr_cmrs,
                ingreso_salida,
                tipo_residuo,
                motivo,
                FechaInicio,
                FechaFin,
                responsable,
                empresa);
        }
    }
}