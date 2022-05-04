using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.RAC
{ 
    public interface IObtenerRegistrosRAC
    {
        Task<List<Domain.RAC.RAC>> Execute(string startTime, string endTime);
    }
}
