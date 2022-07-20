using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida;

namespace GestionAmbiental.Application.UseCases.DashboardGeneral.ResiduosSolidos.IngresoSalidaRS.GetTopGerenciaIngreso
{
    public interface IGetTopGerenciaIngreso
    {
        Task<List<GerenciaIngresoSalida>> Execute(int top, int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string fechaInicio, string fechaFin, int responsable, int empresa);
    }
}