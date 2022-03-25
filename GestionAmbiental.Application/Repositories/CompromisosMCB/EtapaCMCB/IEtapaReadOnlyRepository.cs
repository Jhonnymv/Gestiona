using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.EtapaCMCB
{
    public interface IEtapaReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.EtapaCMCB>> ListarEtapas();

        //Registrar
        Task<int> RegistrarEtapa(Domain.CompromisosMCB.EtapaCMCB etapaCMCB);

        //Editar
        Task EditarEtapa(Domain.CompromisosMCB.EtapaCMCB etapaCMCB);
    }
}
