using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.FactorAmbientalCMCB
{
    public interface IFactorAmbientalReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.FactorAmbientalCMCB>> ListarFactoresAmbientales();
        Task<int> RegistrarFactorAmbiental(Domain.CompromisosMCB.FactorAmbientalCMCB factorAmbientalCMCB);

        Task EditarFactorAmbiental(Domain.CompromisosMCB.FactorAmbientalCMCB factorAmbientalCMCB);
    }
}
