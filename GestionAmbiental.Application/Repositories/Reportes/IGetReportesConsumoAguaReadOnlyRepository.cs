using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesConsumoAguaReadOnlyRepository
    {
        Task<List<Dimension>> GetVolumenFecha(int resolucion, int origen, DateTime FechaInicio, DateTime FechaFin, string frecuencia);
        Task<List<List<Dimension>>> GetVolumenRestante(int resolucion, int origen);
        Task<List<Dimension>> GetCaudalPromedioFecha(int resolucion, int origen, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetTiempoBombeoFecha(int resolucion, int origen, DateTime FechaInicio, DateTime FechaFin);
    }
}
