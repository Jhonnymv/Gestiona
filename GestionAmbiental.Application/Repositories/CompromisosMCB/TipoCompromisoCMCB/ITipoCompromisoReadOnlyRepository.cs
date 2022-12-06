using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.TipoCompromisoCMCB
{
    public interface ITipoCompromisoReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.TipoCompromisoCMCB>> ListarTipoCompromisos();

        Task<int> RegistrarTipoCompromiso(Domain.CompromisosMCB.TipoCompromisoCMCB tipoCompromisoCMCB);

        //Editar
        Task EditarTipoCompromiso(Domain.CompromisosMCB.TipoCompromisoCMCB tipoCompromisoCMCB);
    }
}
