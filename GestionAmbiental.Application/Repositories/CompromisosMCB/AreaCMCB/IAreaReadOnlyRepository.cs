using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB
{
    public interface IAreaReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.AreaCMCB>> ListarAreas();
    }
}
