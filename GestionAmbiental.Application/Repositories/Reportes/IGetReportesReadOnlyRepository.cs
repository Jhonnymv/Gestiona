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
    }
}
