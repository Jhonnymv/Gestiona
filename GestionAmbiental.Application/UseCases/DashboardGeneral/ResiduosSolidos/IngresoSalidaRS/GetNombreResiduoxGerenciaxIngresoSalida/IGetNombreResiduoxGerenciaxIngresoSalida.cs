using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetNombreResiduoxGerenciaxIngresoSalida
{
    public interface IGetNombreResiduoxGerenciaxIngresoSalida
    {
        Task<List<IngresoSalidaNombreResiduo>> Execute(int top,int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int[] residuos, int[] gerencias);
    }
}