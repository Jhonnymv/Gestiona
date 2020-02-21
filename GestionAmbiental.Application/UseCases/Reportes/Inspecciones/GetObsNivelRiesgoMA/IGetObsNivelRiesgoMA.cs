using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsNivelRiesgoMA
{
    public interface IGetObsNivelRiesgoMA
    {
        Task<List<DimensionOutput>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado);
    }
}
