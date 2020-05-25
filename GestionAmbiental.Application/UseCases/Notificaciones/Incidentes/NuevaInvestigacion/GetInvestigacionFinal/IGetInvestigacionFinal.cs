using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.NuevaInvestigacion
{
    public interface IGetInvestigacionFinal
    {
        Task<InvestigacionFinalOutput> Execute(int IdInvestigacion);
    }
}
