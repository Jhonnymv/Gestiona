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
        Task<Dimension> GetObservacionesMAP();
        Task<Dimension> GetObservacionesSSO();
        Task<List<Dimension>> GetObservacionesXUsuario();
        Task<List<Dimension>> GetObservacionesXEstado();
        Task<List<ObservacionesUsuario>> GetResumenObservacionesXUsuario(string filtro);
        Task<List<Dimension>> GetInspeccionesDiarias();
        Task<List<Dimension>> GetObservacionesNivelRiesgoMAP();
        Task<List<Dimension>> GetObservacionesNivelRiesgoSSO();
        Task<List<Dimension>> GetObservacionesTipoInspeccionMAP();
        Task<List<Dimension>> GetObservacionesTipoInspeccionSSO();
        Task<List<DetalleObservacionXEstado>> GetDetalleObservacionXEstados(EstadosInspeccion estado);
        Task<List<List<Dimension>>> GetObservacionesMAPXTipoCondicion();
        Task<List<List<Dimension>>> GetObservacionesSSOXTipoCondicion();

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
