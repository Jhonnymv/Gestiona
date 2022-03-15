using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.EtapaCMCB
{
    public interface ICriterioReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.EtapaCMCB>> ListarEtapas();
    }
}
