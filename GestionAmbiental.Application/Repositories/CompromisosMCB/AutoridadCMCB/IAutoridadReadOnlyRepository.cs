using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.AutoridadCMCB
{
    public interface IAutoridadReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.AutoridadCMCB>> ListarAutoridades();
    }
}
