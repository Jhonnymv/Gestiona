using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAgua
{
    public interface IGetConsumosAgua
    {
        Task<ConsumosAguaOutput> Execute(string fecha, int pagina, int rpp);
    }
}
