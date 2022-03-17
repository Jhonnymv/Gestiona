using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<CompromisosOuput> Execute(int pagina, int rpp, int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas);
        Task<CompromisoOuput> BuscarPorID(int idCompromiso);
    }
}
