using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.ConsumoAgua.ConsumoAguaMar.GetVolumenRestante
{
    public interface IGetVolumenRestante
    {
        Task<List<List<DimensionOutput>>> Execute(int resolucion);
    }
}
