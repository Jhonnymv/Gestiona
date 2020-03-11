using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ResiduosSolidos.ControlIngresoSalida.GetRegistrosIngresoSalida
{
    public interface IGetRegistrosIngresoSalida
    {
        Task<List<DimensionOutput>> Execute(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, string FechaInicio, string FechaFin, int gerencia, int empresa);
    }
}
