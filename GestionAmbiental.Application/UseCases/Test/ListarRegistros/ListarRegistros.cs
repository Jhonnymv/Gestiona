using GestionAmbiental.Application.Repositories.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Test.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly ITestReadOnlyRepository _testReadOnlyRepository;

        public ListarRegistros(ITestReadOnlyRepository testReadOnlyRepository)
        {
            _testReadOnlyRepository = testReadOnlyRepository ?? throw new ArgumentNullException(nameof(testReadOnlyRepository));
        }

        public async Task<TestsOutput> Execute(int pagina, int rpp)
        {
            //Consultar registros de BD
            var registros = await _testReadOnlyRepository.ListarTest();

            //Nueva lista en blanco
            var output = new TestsOutput();
            output.Registros = new List<TestOutput>();

            //Mapear registros en nueva lista
            foreach(var item in registros)
            {
                output.Registros.Add(new TestOutput
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Numero = item.Numero,
                    Evidencia = item.Evidencia
                });
            }
            //Contar registros
            output.NRegistros = output.Registros.Count;
            //Filtrar registros en funcion a la pagina y registros por pagina
            output.Registros = output.Registros.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            //Retornar registros
            return output;
        }
    }
}
