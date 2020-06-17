using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminarMA.EliminarEvindenciaReportePreliminar
{
    public interface IEliminarEvindenciaReportePreliminar
    {
        Task Execute(int IdReporte, string NombreArchivo);
    }
}
