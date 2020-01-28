using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetDetalleObservacionesXEstado
{
    public class GetDetalleObservacionesXEstado : IGetDetalleObservacionesXEstado
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesReadOnlyRepository;

        public GetDetalleObservacionesXEstado(IGetReportesInspeccionesReadOnlyRepository getReportesReadOnlyRepository)
        {
            _getReportesReadOnlyRepository = getReportesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesReadOnlyRepository));
        }

        public async Task<List<DetalleObservacionXEstado>> Execute(string Estado)
        {
            var output = new List<DetalleObservacionXEstado>();
            switch (Estado)
            {
                case "Registrado": output = await _getReportesReadOnlyRepository.GetDetalleObservacionXEstados(EstadosInspeccion.Registrado);break;
                case "Implementado": output = await _getReportesReadOnlyRepository.GetDetalleObservacionXEstados(EstadosInspeccion.Implementado); break;
                case "Vencido": output = await _getReportesReadOnlyRepository.GetDetalleObservacionXEstados(EstadosInspeccion.Vencido); break;
                case "Ampliado": output = await _getReportesReadOnlyRepository.GetDetalleObservacionXEstados(EstadosInspeccion.Ampliado); break;
                case "Aprobado": output = await _getReportesReadOnlyRepository.GetDetalleObservacionXEstados(EstadosInspeccion.Aprobado); break;
                case "Rechazado": output = await _getReportesReadOnlyRepository.GetDetalleObservacionXEstados(EstadosInspeccion.Rechazado); break;
            }
            return output;
        }
    }
}
