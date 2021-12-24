using GestionAmbiental.Application.Repositories.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.EliminarRegistro
{
    public class EliminarRegistro : IEliminarRegistro
    {
        private readonly ITestReadOnlyRepository _testReadOnlyRepository;

        public EliminarRegistro(ITestReadOnlyRepository testReadOnlyRepository)
        {
            _testReadOnlyRepository = testReadOnlyRepository ?? throw new ArgumentNullException(nameof(testReadOnlyRepository));
        }

        public async Task Execute(int id)
        {
            await _testReadOnlyRepository.EliminarTest(id);
        }
    }
}
