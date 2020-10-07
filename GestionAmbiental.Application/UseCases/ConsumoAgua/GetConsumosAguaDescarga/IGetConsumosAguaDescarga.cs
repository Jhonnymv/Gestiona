using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAguaDescarga
{
    public interface IGetConsumosAguaDescarga
    {
        Task<List<ConsumoAguaOutput>> ExecuteFechas(string fechaInicio, string fechaFin, int resolucion);
        Task<List<ConsumoMensualOutput>> ExecuteMensual(int resolucion,int anio);
    }
}
