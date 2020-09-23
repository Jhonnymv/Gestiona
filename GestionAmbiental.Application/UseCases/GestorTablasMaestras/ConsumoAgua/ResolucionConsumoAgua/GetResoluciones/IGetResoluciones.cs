using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.GetResoluciones
{
    public interface IGetResoluciones
    {
        Task<ResolucionesConsumoAguaOutput> Execute(string filtro, int pagina, int rpp);
    }
}
