using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones
{
    public interface IGetAccionesIncidenteInvestigacion
    {
        Task<List<AccioneIncidenteOutput>> Execute(int IdInvestigacion);
        Task<List<AccioneIncidenteOutput>> Execute(int IdInvestigacion,string usuario_red);
    }
}
