using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObsMAPvsSSO_Estado
{
    public interface IGetObsMAPvsSSO_Estado
    {
        Task<List<List<DimensionOutput>>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion);
    }
}
