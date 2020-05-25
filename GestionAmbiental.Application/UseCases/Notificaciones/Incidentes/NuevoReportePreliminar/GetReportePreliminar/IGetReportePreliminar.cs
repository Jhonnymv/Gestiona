using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.NuevoReportePreliminar.GetReportePreliminar
{
    public interface IGetReportePreliminar
    {
        Task<ReportePreliminarOutput> Execute(int IdReporte);
    }
}
