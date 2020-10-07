using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetRegistroRecienteConsumoAgua
{
    public interface IGetRegistrosRecientesConsumoAgua
    {
        Task<List<ConsumoAguaOutput>> Execute(int[] IdResolucion);
    }
}
