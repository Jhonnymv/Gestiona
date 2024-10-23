using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetGerenciaInspeccionadaxEstadoObs
{
    public interface IGetGerenciaInspeccionadaxEstadoObs
    {
        Task<List<List<DimensionOutput>>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
    }
}
