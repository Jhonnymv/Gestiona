using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.EliminarAccion
{
    public interface IEliminarAccionIncidenteInvestigacion
    {
        Task Execute(int IdAccion);
    }
}
