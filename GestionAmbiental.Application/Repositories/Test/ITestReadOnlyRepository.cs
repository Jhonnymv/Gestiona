using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Test
{
    public interface ITestReadOnlyRepository
    {
        Task<int> RegistrarTest(Domain.Test.Test input);
        Task EditarTest(Domain.Test.Test input);
        Task<List<Domain.Test.Test>> ListarTest();
        Task EliminarTest(int IdElimnado);
    }
}
