using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones
{
    public interface IGetAccioIncidente
    {
        Task<AccioneIncidenteOutput> Execute(int IdAccion);
    }
}
