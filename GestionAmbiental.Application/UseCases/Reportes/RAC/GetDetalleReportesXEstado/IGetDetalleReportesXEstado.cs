using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetDetalleReportesXEstado
{
    public interface IGetDetalleReportesXEstado
    {
        Task<ListRacOutput> Execute(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin, string estado);
    }
}
