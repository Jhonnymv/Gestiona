using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.EliminarCausa
{
    public interface IEliminarCausaIncidenteInvestigacion
    {
        Task Execute(int Id_Causa_Incidente);
    }
}
