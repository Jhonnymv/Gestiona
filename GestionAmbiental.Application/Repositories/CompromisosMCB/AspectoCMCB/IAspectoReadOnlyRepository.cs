using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.AspectoCMCB
{
    public interface IAspectoReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.AspectoCMCB>> ListarAspectos();
    }
}
