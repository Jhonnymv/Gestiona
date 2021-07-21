using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EliminarReportePreliminar
{
    public interface IEliminarReportePreliminar
    {
        Task<int> Execute(int IdReportePreliminar);
    }
}
