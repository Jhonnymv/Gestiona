using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesTipoCondicionxNivelRiesgoMA
{
    public interface IGetReportesTipoCondicionxNivelRiesgoMA
    {
        Task<List<List<DimensionOutput>>> Execute(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin);
    }
}
