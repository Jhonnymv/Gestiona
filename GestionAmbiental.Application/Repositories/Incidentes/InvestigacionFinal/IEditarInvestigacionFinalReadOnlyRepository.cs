using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal
{
    public interface IEditarInvestigacionFinalReadOnlyRepository
    {
        Task<int> EditarProductoDerramado(Producto_Derramado_Incidente producto);
        Task<int> EditarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion);
        Task<int> ImplementarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion);
        Task<int> VerificarAccionCorrectivaInvestigacion(Accion_Incidente_Investigacion accion);
        Task<int> EditarCausaIncidenteInvestigacion(Causa_Incidente_Investigacion causa);
        Task<int> EditarInvolucradoInvestigacion(Involucrado_Incidente_Investigacion involucrado);
        Task<int> EditarInvestigacionFinal(Investigacion_Final_Incidente investigacion, Impacto_Ambiental_Incidente impacto);
        Task<int> EditarAdminInvestigacion(Investigacion_Final_Incidente investigacion);
        Task<int> AmpliarFechaAccion(Accion_Incidente_Investigacion accion);
        Task EliminarEvidenciaInvestigacion(int IdInvestigacion, string Tipo, string NombreArchivo);
    }
}
