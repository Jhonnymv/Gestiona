using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Implementacion.ConsultarRACs
{
    public class ConsultarRACs : IConsultarRACs
    {
        private readonly IGetRACReadOnlyRepository _RACReadOnlyRepository;
        public ConsultarRACs(IGetRACReadOnlyRepository rACReadOnlyRepository)
        {
            _RACReadOnlyRepository = rACReadOnlyRepository ?? throw new ArgumentNullException(nameof(rACReadOnlyRepository));
        }
        public async Task<RACsOutput> Execute(int pagina, int fpp, DateTime fecha, string codigo)
        {
            var registros = await _RACReadOnlyRepository.GetRACs(fecha, codigo);
            var output = new RACsOutput
            {
                Registros = new List<RACOutput>(),
                NRegistros = registros.Count
            };
            registros = registros.Skip((pagina - 1) * fpp).Take(fpp).ToList();
            foreach (var item in registros)
            {
                output.Registros.Add(new RACOutput(item));
            }
            return output;
        }
    }
}
