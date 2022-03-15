using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB
{
    public interface ISustentoReadOnlyRepository
    {

        //Listar
        Task<List<Domain.CompromisosMCB.SustentoCMCB>> ListarSustentos();
    }
}
