using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal
{
    public interface IGetInvestigacionFinalReadOnlyRepository
    {
        Task<List<Investigacion_Final_Incidente>> GetInvestigacionesFinales(DateTime? fechaInicio, DateTime? fechaFin, int[] gerencia, int titular, List<string> estado, int[] responsable, int[] asesor, string codigo);
        Task<Investigacion_Final_Incidente> GetInvestigacionFinal(int IdInvestigacion);
        Task<Impacto_Ambiental_Incidente> GetImpactoAmbiental(int IdInvestigacion);
        Task<string> GetEstadoInvestigacion(int IdInvestigacion);
        Task<string> GetEvidenciaInvestigacion(int IdInvestigacion, string Tipo);
        Task<string> GetEvidenciasNombreInvestigacion(int IdInvestigacion);
    }
}
