using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.GetFuentesAgua
{
    public interface IGetFuentesAgua
    {
        Task<FuentesAguaOutput> Execute(string filtro, int pagina, int rpp);
        Task<FuentesAguaOutput> Execute();
    }
}
