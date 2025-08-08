using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar
{
    public interface IGetReportePreliminar
    {
        Task<ReportePreliminarOutput> Execute(int IdReporte);
        Task<ReportePreliminarOutput> Execute(int IdReporte,string usuario_red);
    }
}
