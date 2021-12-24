using GestionAmbiental.Application.Repositories.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly ITestReadOnlyRepository _testReadOnlyRepository;

        public EditarRegistro(ITestReadOnlyRepository testReadOnlyRepository)
        {
            _testReadOnlyRepository = testReadOnlyRepository ?? throw new ArgumentNullException(nameof(testReadOnlyRepository));
        }

        public async Task Execute(int id, string nombre, int? numero, string evidencia)
        {
            var input = new Domain.Test.Test{
                Id = id,
                Nombre = nombre,
                Numero= numero,
                Evidencia = evidencia
            };

            await _testReadOnlyRepository.EditarTest(input);
        }
    }
}
