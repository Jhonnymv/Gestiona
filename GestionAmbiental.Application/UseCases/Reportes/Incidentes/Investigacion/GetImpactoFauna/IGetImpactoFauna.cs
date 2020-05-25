using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes.Investigacion.GetImpactoFauna
{
    public interface IGetImpactoFauna
    {
        Task<List<DimensionOutput>> Execute(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin);
    }
}
