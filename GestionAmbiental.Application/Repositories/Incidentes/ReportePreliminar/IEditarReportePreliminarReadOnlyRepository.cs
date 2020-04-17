using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar
{
    public interface IEditarReportePreliminarReadOnlyRepository
    {
        Task<int> EditarReportePreliminar(Reporte_Preliminar_Incidente reporte);
    }
}
