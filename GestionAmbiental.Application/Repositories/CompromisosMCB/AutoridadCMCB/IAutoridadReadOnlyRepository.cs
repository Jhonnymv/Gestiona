using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.AutoridadCMCB
{
    public interface IAutoridadReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.AutoridadCMCB>> ListarAutoridades();
        //Registrar
        Task<int> RegistrarAutoridad(Domain.CompromisosMCB.AutoridadCMCB autoridadCMCB);

        //Editar
        Task EditarAutoridad(Domain.CompromisosMCB.AutoridadCMCB autoridadCMCB);
    }
}
