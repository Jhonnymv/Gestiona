using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesInspeccionesReadOnlyRepository
    {
        Task<List<Dimension>> GetNInspecciones(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetInspeccionesXFecha(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string fecuencia);

        /*
        Task<List<Dimension>> GetObservacionesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Domain.Reportes.DetalleObservacionXEstado>> GetDetalleReportesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin, EstadosRAC estado);
        Task<List<Dimension>> GetInspeccionesXFecha(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetObservacionesNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetObservacionesNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetObservacionesClasificacionMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetObservacionesClasificacionSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<List<List<Dimension>>> GetObservacionesTipoCondicionxNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        Task<List<List<Dimension>>> GetRObservacionesTipoCondicionxNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin);
        */
    }
}
