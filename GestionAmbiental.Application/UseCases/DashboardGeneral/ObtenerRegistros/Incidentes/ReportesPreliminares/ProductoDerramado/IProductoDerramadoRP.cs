using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.ReportesPreliminares.ProductoDerramado
{
    public interface IProductoDerramadoRP
    {
        Task<List<Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente>> Execute(string startTime, string endTime);
    }
}
