using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesInvestigacionIncidenteReadOnlyRepository
    {
        Task<List<Dimension>> GetNInvestigacion(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin, string frecuencia);
        Task<List<Dimension>> GetNInvestigacionesTipoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNInvestigacionesNivelIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNInvestigacionesCausasInmediatasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNInvestigacionesCausasBasicasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNInvestigacionesGerenciaIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<List<Dimension>>> GetCantidadProductoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetImpactoSuelo(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetResiduoImpactoSuelo(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetImpactoFlora(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetResiduoImpactoFlora(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetImpactoFauna(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetResiduoImpactoFauna(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetImpactoAgua(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetResiduoImpactoAgua(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetImpactoPersona(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin);
    }
}
