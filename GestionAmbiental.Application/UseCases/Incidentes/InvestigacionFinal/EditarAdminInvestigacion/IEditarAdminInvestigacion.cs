using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EditarAdminInvestigacion
{
    public interface IEditarAdminInvestigacion
    {
        Task<int> Execute(int IdInvestigacion,
            int IdSupervisor,
            List<string> Evidencias_Papel,
            List<string> Evidencias_Persona,
            List<string> Evidencias_Posicion,
            List<string> Evidencias_Partes);
    }
}
