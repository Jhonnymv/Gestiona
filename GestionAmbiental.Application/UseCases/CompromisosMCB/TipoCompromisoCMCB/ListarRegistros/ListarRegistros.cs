using GestionAmbiental.Application.Repositories.CompromisosMCB.AreaCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.TipoCompromisoCMCB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.TipoCompromisoCMCB.ListarRegistros
{
    public class ListarRegistros:IListarRegistros
    {
        private readonly ITipoCompromisoReadOnlyRepository _tipoCompromisoReadOnlyRepository;

        public ListarRegistros(ITipoCompromisoReadOnlyRepository tipoCompromisoReadOnlyRepository)
        {
            _tipoCompromisoReadOnlyRepository = tipoCompromisoReadOnlyRepository ?? throw new ArgumentNullException(nameof(tipoCompromisoReadOnlyRepository));
        }

        public async Task<TipoCompromisosOuput> Execute()
        {
            var registros = await _tipoCompromisoReadOnlyRepository.ListarTipoCompromisos();

            var output = new TipoCompromisosOuput();
            output.Registros = new List<TipoCompromisoOuput>();
            foreach (var registro in registros)
            {
                output.Registros.Add(new TipoCompromisoOuput()
                {
                    Id=registro.Id,
                    Tipo=registro.Tipo,
                    Descripcion = registro.Descripcion
                });
            }
            output.NRegistros = output.Registros.Count;


            return output;
        }
    }
}
