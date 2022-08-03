using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Inspecciones
{
    public interface IObtenerRegistrosInspecciones
    {
        Task<List<Domain.Inspecciones.InspeccionDetalle>> Execute(string startTime, string endTime);
    }
}
