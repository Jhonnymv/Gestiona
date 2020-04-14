using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EditarReportePreliminar
{
    public interface IGetReportePreliminar
    {
        Task<ReportePreliminarOutput> Execute(int IdReporte);
    }
}
