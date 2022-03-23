using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CriterioCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CriterioCMBC.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly ICriterioReadOnlyRepository _creiterioReadOnlyRepository;

        public ListarRegistros(ICriterioReadOnlyRepository criterioReadOnlyRepository)
        {
            _creiterioReadOnlyRepository = criterioReadOnlyRepository ?? throw new ArgumentNullException(nameof(criterioReadOnlyRepository));
        }

        public async Task<CriteriosOuput> Execute()
        {
            //Consultar registros de BD
            var registros = await _creiterioReadOnlyRepository.ListarCriterios();

            //Nueva lista en blanco
            var output = new CriteriosOuput();
            output.Registros = new List<CriterioOuput>();

            //Mapear registros en nueva lista
            foreach (var criterio in registros)
            {
                output.Registros.Add(new CriterioOuput
                {
                    Id= criterio.Id,
                    Criterio= criterio.Criterio,
                    Detalle=criterio.Detalle
                });
            }

            //Contar registros
            output.NRegistros = output.Registros.Count;

            //Retornar registros
            return output;
        }
    }
}
