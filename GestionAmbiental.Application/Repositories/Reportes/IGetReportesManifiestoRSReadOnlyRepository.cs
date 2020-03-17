using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesManifiestoRSReadOnlyRepository
    {
        Task<List<List<Dimension>>> GetNManifiestosFechas(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada, string frecuencia);
        Task<List<Dimension>> GetNManifiestosObservacion(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada);
        Task<List<Dimension>> GetNManifiestosCamion(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada);
        Task<List<Dimension>> GetManifiestosTipoSalida(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada);
        Task<List<Dimension>> GetManifiestosTipoLlegada(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada);
    }
}
