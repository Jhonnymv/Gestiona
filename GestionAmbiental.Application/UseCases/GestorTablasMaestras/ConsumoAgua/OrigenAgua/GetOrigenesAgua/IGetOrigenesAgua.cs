using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.GetOrigenesAgua
{
    public interface IGetOrigenesAgua
    {
        Task<OrigenesAguaOutput> Execute(string filtro, int pagina, int rpp);
        Task<OrigenesAguaOutput> Execute();
    }
}
