using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.ComponenteCMCB
{
    public interface IComponeteReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.ComponenteCMCB>> ListarComponentes();

        //Registrar
        Task<int> RegistrarComponente(Domain.CompromisosMCB.ComponenteCMCB componenteCMCB);

        //Editar
        Task EditarComponente(Domain.CompromisosMCB.ComponenteCMCB componenteCMCB);
    }
}
