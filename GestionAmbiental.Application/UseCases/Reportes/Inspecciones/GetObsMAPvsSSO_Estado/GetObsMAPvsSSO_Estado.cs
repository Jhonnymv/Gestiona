using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsMAPvsSSO_Estado
{
    public class GetObsMAPvsSSO_Estado : IGetObsMAPvsSSO_Estado
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesInspeccionesReadOnlyRepository;
        public GetObsMAPvsSSO_Estado(IGetReportesInspeccionesReadOnlyRepository getReportesInspeccionesReadOnlyRepository)
        {
            _getReportesInspeccionesReadOnlyRepository = getReportesInspeccionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInspeccionesReadOnlyRepository));
        }
        public async Task<List<List<DimensionOutput>>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado)
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
            var data = await _getReportesInspeccionesReadOnlyRepository.GetObsMAPvsSSO_Estado(empresa, gerencia, respLevObs, Asesor, fInicio, fFinal, enum_estado);

            var output = new List<List<DimensionOutput>>();
            
            foreach (var item in data)
            {
                var temp = new List<DimensionOutput>();
                foreach(var dimen in item)
                {
                    temp.Add(new DimensionOutput(dimen));
                }
                output.Add(temp);
            }
            return output;
        }
    }
}
