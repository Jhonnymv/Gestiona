using GestionAmbiental.Application.Repositories.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly ITestReadOnlyRepository _testReadOnlyRepository;

        public NuevoRegistro(ITestReadOnlyRepository testReadOnlyRepository)
        {
            _testReadOnlyRepository = testReadOnlyRepository ?? throw new ArgumentNullException(nameof(testReadOnlyRepository));
        }

        public async Task<int> Execute(string nombre, int? numero, string evidencia)
        {
            var input = new Domain.Test.Test
            {
                Nombre = nombre,
                Numero = numero,
                Evidencia = evidencia
            };

            return await _testReadOnlyRepository.RegistrarTest(input);
        }
    }
}
