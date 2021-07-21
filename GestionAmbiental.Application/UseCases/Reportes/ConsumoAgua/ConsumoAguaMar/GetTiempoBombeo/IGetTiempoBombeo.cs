using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaMar.GetTiempoBombeo
{
    public interface IGetTiempoBombeo
    {
        Task<List<DimensionOutput>> Execute(int resolucion, string FechaInicio, string FechaFin, string frecuencia);
    }
}
