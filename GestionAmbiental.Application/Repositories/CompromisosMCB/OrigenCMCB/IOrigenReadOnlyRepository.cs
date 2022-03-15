using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.OrigenCMCB
{
    public interface IOrigenReadOnlyRepository
    {
        //Listar
        Task<List<Domain.CompromisosMCB.OrigenCMCB>> ListarOrigenes();
    }
}
