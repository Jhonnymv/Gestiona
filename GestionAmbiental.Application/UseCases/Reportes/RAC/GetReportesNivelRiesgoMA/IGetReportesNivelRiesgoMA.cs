using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetReportesNivelRiesgoMA
{
    public interface IGetReportesNivelRiesgoMA
    {
        Task<List<DimensionOutput>> Execute(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin);
    }
}
