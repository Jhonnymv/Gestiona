using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal
{
    public interface IRegistrarInvestigacionFinalReadOnlyRepository
    {
        Task<int> RegistrarDeReportePreliminar(int IdReporte);
        Task<int> RegistrarImpactoAmbiental(int IdInvestigacion);
        Task<int> RegistrarProductoDerramado(Producto_Derramado_Incidente producto);
        Task<int> RegistrarInvolucradoInvestigacion(Involucrado_Incidente_Investigacion involucrado);
        Task<int> RegistrarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion);
        Task<int> RegistrarCausaIncidenteInvestigacion(Causa_Incidente_Investigacion causa);
    }
}
