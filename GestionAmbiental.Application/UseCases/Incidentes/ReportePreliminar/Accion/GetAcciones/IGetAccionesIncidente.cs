using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones
{
    public interface IGetAccionesIncidente
    {
        Task<List<AccioneIncidenteOutput>> Execute(int IdReportePreliminar);
    }
}
