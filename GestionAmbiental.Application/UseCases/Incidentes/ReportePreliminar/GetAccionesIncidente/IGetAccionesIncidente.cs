using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetAccionesIncidente
{
    public interface IGetAccionesIncidente
    {
        Task<List<AccioneIncidenteOutput>> Execute(int IdReportePreliminar);
    }
}
