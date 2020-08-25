using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesAvistamientoFaunaReadOnlyRepository
    {
        Task<List<Dimension>> GetNRegistrosEntreFechas(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin,string frecuencia);
        Task<List<Dimension>> GetNRegistrosNombreComun(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNRegistrosTipoAvistamiento(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin);
        Task<List<Dimension>> GetNRegistrosEstadoFauna(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin);
    }
}
