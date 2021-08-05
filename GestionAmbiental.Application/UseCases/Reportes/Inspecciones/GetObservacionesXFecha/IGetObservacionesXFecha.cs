using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXFecha
{
    public interface IGetObservacionesXFecha
    {
        Task<List<List<DimensionOutput>>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string frecuencia,string estado);
    }
}
