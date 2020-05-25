using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Causa.EliminarCausa
{
    public interface IEliminarCausaIncidenteReporte
    {
        Task Execute(int Id_Causa_Incidente);
    }
}
