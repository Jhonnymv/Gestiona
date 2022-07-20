using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxDestinoxIngresoSalida
{
    public interface IGetNombreResiduoxDestinoxIngresoSalida
    {
        Task<List<IngresoSalidaNombreResiduo>> Execute(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int[] residuos, int[] destinos);
    }
}