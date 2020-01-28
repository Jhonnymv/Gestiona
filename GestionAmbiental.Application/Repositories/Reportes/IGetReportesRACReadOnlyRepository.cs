using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesRACReadOnlyRepository
    {
        Task<List<Dimension>> GetObservacionesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario);
        Task<List<Dimension>> GetNReportesEntreFechas(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
    }
}
