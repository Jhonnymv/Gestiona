using GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.FactorAmbientalCMCB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.FactorAmbientalCMCB.ListarRegistros
{
    public class ListarRegistros:IListarRegistros
    {
        public readonly IFactorAmbientalReadOnlyRepository _factorAmbientalReadOnlyRepository;
        public ListarRegistros(IFactorAmbientalReadOnlyRepository factorAmbientalReadOnlyRepository)
        {
            _factorAmbientalReadOnlyRepository = factorAmbientalReadOnlyRepository ?? throw new ArgumentNullException(nameof(factorAmbientalReadOnlyRepository));
        }

        public async Task<FactoresAmbientalesOuput> Execute()
        {
            var registros = await _factorAmbientalReadOnlyRepository.ListarFactoresAmbientales();

            var output = new FactoresAmbientalesOuput();

            output.Registros = new List<FactorAmbientalOuput>();

            foreach(var item in registros)
            {
                output.Registros.Add(new FactorAmbientalOuput
                {
                    Id= item.Id,
                    Factor= item.Factor,
                    descripcion= item.Descripcion,
                });
            }

            output.NRegistros = output.Registros.Count;

            return output;
        }
    }
}
