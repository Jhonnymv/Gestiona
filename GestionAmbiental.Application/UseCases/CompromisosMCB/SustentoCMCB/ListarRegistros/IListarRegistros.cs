using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<SustentosOuput> Execute(int pagina, int fpp, int[] gerencias, int [] areas, int [] criterios, string [] estados, int[] etapas);

        Task<SustentoOuput> ObtenerSustento(int Id);

        Task<SustentosOuput> GetSustentosByCompromiso(int IdCompromiso);
    }
}
