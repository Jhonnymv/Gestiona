using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesReadOnlyRepository
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
    }
}
