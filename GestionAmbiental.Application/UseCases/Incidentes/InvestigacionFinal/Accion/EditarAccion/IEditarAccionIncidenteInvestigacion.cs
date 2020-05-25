using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.EditarAccion
{
    public interface IEditarAccionIncidenteInvestigacion
    {
        Task<int> Execute(int IdAccion, int IdInvestigacion,string tipoAccion, int IdUsuario, string AccionInmediata, string Fecha, string Responsable, string Estatus);
    }
}
