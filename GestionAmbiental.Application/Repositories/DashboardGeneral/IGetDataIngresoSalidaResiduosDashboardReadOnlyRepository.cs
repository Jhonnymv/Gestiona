using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida;

namespace GestionAmbiental.Application.Repositories.DashboardGeneral
{
    public interface IGetDataIngresoSalidaResiduosDashboardReadOnlyRepository
    {
        Task<List<NombreResiduoxIngresoSalida>> GetNombreResiduoxIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa);
        Task<List<DestinoResiduos>> GetDestinoxNombreResiduoxIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa);
        Task<List<IngresoSalidaNombreResiduo>> GetNombreResiduoxDestinoxIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int[] residuos, int[] destinos);
        Task<List<GerenciaIngresoSalida>> GetGerenciaIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa);
        Task<List<IngresoSalidaNombreResiduo>> GetNombreResiduoxGerencia(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int[] residuos, int[] gerencias);
    }
}