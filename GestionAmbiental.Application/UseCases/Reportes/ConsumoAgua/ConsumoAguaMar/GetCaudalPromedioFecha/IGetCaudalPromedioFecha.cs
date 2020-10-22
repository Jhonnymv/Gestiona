using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaMar.GetCaudalPromedioFecha
{
    public interface IGetCaudalPromedioFecha
    {
        Task<List<DimensionOutput>> Execute(int resolucion, string FechaInicio, string FechaFin,string frecuencia);
    }
}
