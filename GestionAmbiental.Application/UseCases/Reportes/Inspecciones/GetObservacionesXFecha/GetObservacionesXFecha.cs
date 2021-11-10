using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXFecha
{
    public class GetObservacionesXFecha : IGetObservacionesXFecha
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesInspeccionesReadOnlyRepository;
        public GetObservacionesXFecha(IGetReportesInspeccionesReadOnlyRepository getReportesInspeccionesReadOnlyRepository)
        {
            _getReportesInspeccionesReadOnlyRepository = getReportesInspeccionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInspeccionesReadOnlyRepository));
        }
        public async Task<List<List<DimensionOutput>>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string frecuencia, string estado, int idGerenciaInspeccionada)
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
            //    case "Reg":enum_estado = EstadosInspeccion.Registrado;break;
            //    case "Imp": enum_estado = EstadosInspeccion.Implementado; break;
            //    case "Ven": enum_estado = EstadosInspeccion.Vencido; break;
            //    case "Apr": enum_estado = EstadosInspeccion.Aprobado; break;
            //    case "Rec": enum_estado = EstadosInspeccion.Rechazado; break;
            //}
            var data = await _getReportesInspeccionesReadOnlyRepository.GetObservacionesXFecha(empresa, gerencia, respLevObs, Asesor, fInicio, fFinal, frecuencia,estado,idGerenciaInspeccionada);

            var output = new List<List<DimensionOutput>>();
            var outputObs = new List<DimensionOutput>();
            var outputBP = new List<DimensionOutput>();
            foreach (var item in data[0])
            {
                outputObs.Add(new DimensionOutput(item));
            }
            foreach (var item in data[1])
            {
                outputBP.Add(new DimensionOutput(item));
            }
            output.Add(outputObs);
            output.Add(outputBP);
            return output;
        }
    }
}
