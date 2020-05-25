using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoPersona
{
    public interface IGetImpactoPersona
    {
        Task<List<DimensionOutput>> Execute(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
    }
}
