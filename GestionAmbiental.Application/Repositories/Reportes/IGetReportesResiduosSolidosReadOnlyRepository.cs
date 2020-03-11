using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetReportesResiduosSolidosReadOnlyRepository
    {
        Task<List<List<Dimension>>> GetNRegistrosEntreFechas(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa, string frecuencia);
        Task<List<Dimension>> GetNRegistrosACR_CMRS(int acr_cmrs,int ingreso_salida,int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin,int responsable, int empresa);
        Task<List<Dimension>> GetNRegistrosIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa);
        Task<List<Dimension>> GetNRegistrosTipoResiduo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa);
        Task<List<Dimension>> GetNRegistrosMotivo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa);
    }
}
