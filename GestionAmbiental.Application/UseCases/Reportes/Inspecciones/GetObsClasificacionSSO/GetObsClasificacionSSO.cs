using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsClasificacionSSO
{
    public class GetObsClasificacionSSO : IGetObsClasificacionSSO
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesInspeccionesReadOnlyRepository;

        public GetObsClasificacionSSO(IGetReportesInspeccionesReadOnlyRepository getReportesInspeccionesReadOnlyRepository)
        {
            _getReportesInspeccionesReadOnlyRepository = getReportesInspeccionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInspeccionesReadOnlyRepository));
        }

        public async Task<List<DimensionOutput>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            EstadosInspeccion? enum_estado = null;
            switch (estado)
            {
                case "Reg": enum_estado = EstadosInspeccion.Registrado; break;
                case "Imp": enum_estado = EstadosInspeccion.Implementado; break;
                case "Ven": enum_estado = EstadosInspeccion.Vencido; break;
                case "Apr": enum_estado = EstadosInspeccion.Aprobado; break;
                case "Rec": enum_estado = EstadosInspeccion.Rechazado; break;
                case "Eli": enum_estado = EstadosInspeccion.Eliminado; break;
            }
            var data = await _getReportesInspeccionesReadOnlyRepository.GetObsClasificacionSSO(empresa, gerencia, respLevObs, Asesor, fInicio, fFinal, enum_estado);

            var output = new List<DimensionOutput>();
            foreach (var item in data)
            {
                output.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
