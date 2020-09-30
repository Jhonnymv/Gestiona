using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaSubterranea.GetVolumenFecha
{
    public interface IGetVolumenFecha
    {
        Task<List<DimensionOutput>> Execute(int resolucion, string FechaInicio, string FechaFin, string frecuencia);
    }
}
