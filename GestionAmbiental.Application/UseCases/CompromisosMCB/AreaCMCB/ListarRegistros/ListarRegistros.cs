using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.AreaCMCB.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly IAreaReadOnlyRepository _areaReadOnlyRepository;

        public ListarRegistros(IAreaReadOnlyRepository areaReadOnlyRepository)
        {
            _areaReadOnlyRepository = areaReadOnlyRepository ?? throw new ArgumentNullException(nameof(areaReadOnlyRepository));
        }

        public async Task<AreasOuput> Execute()
        {
            //Consultar registros de BD
            var registros = await _areaReadOnlyRepository.ListarAreas();

            //Nueva lista en blanco
            var output = new AreasOuput();
            output.Registros = new List<AreaOuput>();

            //Mapear registros en nueva lista
            foreach (var area in registros)
            {
                output.Registros.Add(new AreaOuput
                {
                    Id=area.Id,
                    Area=area.Area,
                });
            }

            //Contar registros
            output.NRegistros = output.Registros.Count;

            //Retornar registros
            return output;
        }
    }
}
