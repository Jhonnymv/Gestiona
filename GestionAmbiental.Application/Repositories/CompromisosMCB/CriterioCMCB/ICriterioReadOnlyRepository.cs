using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.CriterioCMCB
{
    public interface ICriterioReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.CriterioCMCB>> ListarCriterios();

        //Registrar
        Task<int> RegistrarCriterio(Domain.CompromisosMCB.CriterioCMCB criterioCMCB);
    }
}
