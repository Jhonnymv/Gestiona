using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones
{
    public interface IGetAccioIncidenteInvestigacion
    {
        Task<AccioneIncidenteOutput> Execute(int IdAccion);
    }
}
