using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.DashboardGeneral
{
    public interface IGetRegistrosReadOnlyRepository
    {
        Task<List<Domain.Inspecciones.InspeccionDetalle>> Observaciones(DateTime startTime, DateTime endTime);
        Task<List<Domain.RAC.RAC>> RACs(DateTime startTime, DateTime endTime);
    }
}
