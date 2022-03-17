using GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.ListarRegistros
{
    public class ListarRegistros : IListarRegistros
    {
        private readonly ISustentoReadOnlyRepository _sustentoReadOnlyRepository;

        public ListarRegistros(ISustentoReadOnlyRepository sustentoReadOnlyRepository)
        {
            _sustentoReadOnlyRepository = sustentoReadOnlyRepository ?? throw new ArgumentNullException(nameof(sustentoReadOnlyRepository));
        }

        public async Task<SustentosOuput> Execute(int pagina, int fpp, int[] gerencias, int[] areas, int[] criterios, string[] estados)
        {
            var data = await _sustentoReadOnlyRepository.ListarSustentos(gerencias,areas,criterios,estados);
            var output = new SustentosOuput();

            var sustentos = new List<SustentoOuput>();
            foreach (var item in data)
            {
                sustentos.Add(new SustentoOuput(item));
            }

            output.Registros = sustentos.Skip((pagina - 1) * fpp).Take(fpp).ToList();
            output.NRegistros = sustentos.Count;

            return output;
        }
    }
}
