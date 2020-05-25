using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetNInvestigacionesCausasInmediatasIncidente
{
    public interface IGetNInvestigacionesCausasInmediatasIncidente
    {
        Task<List<DimensionOutput>> Execute(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
    }
}
