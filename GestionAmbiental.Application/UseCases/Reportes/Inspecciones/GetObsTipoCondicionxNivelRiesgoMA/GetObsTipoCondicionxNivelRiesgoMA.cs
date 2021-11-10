using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsTipoCondicionxNivelRiesgoMA
{
    public class GetObsTipoCondicionxNivelRiesgoMA : IGetObsTipoCondicionxNivelRiesgoMA
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesInspeccionesReadOnlyRepository;

        public GetObsTipoCondicionxNivelRiesgoMA(IGetReportesInspeccionesReadOnlyRepository getReportesInspeccionesReadOnlyRepository)
        {
            _getReportesInspeccionesReadOnlyRepository = getReportesInspeccionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInspeccionesReadOnlyRepository));
        }

        public async Task<List<List<DimensionOutput>>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado, int idGerenciaInspeccionada)
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
            //EstadosInspeccion? enum_estado = null;
            //switch (estado)
            //{
            //    case "Reg": enum_estado = EstadosInspeccion.Registrado; break;
            //    case "Imp": enum_estado = EstadosInspeccion.Implementado; break;
            //    case "Ven": enum_estado = EstadosInspeccion.Vencido; break;
            //    case "Apr": enum_estado = EstadosInspeccion.Aprobado; break;
            //    case "Rec": enum_estado = EstadosInspeccion.Rechazado; break;
            //}
            var data = await _getReportesInspeccionesReadOnlyRepository.GetObsTipoCondicionxNivelRiesgoMA(empresa, gerencia, respLevObs, Asesor, fInicio, fFinal,estado,idGerenciaInspeccionada);

            var output = new List<List<DimensionOutput>>();

            foreach (var item in data)
            {
                var temp = new List<DimensionOutput>();
                foreach (var dimen in item)
                {
                    temp.Add(new DimensionOutput(dimen));
                }
                output.Add(temp);
            }
            return output;
        }
    }
}
