using GestionAmbiental.Application.Repositories.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetRegistroRecienteConsumoAgua
{
    public class GetRegistrosRecientesConsumoAgua : IGetRegistrosRecientesConsumoAgua
    {
        private readonly IGetConsumoAguaReadOnlyRepository _getConsumoAguaReadOnlyRepository;

        public GetRegistrosRecientesConsumoAgua(IGetConsumoAguaReadOnlyRepository getConsumoAguaReadOnlyRepository)
        {
            _getConsumoAguaReadOnlyRepository = getConsumoAguaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getConsumoAguaReadOnlyRepository));
        }

        public async Task<List<ConsumoAguaOutput>> Execute(int[] IdResoluciones)
        {
            var output = new List<ConsumoAguaOutput>();
            foreach (var resolucion in IdResoluciones)
            {
                var data = await _getConsumoAguaReadOnlyRepository.GetRecienteConsumoAgua(resolucion);
                if(data!=null)
                output.Add(new ConsumoAguaOutput(data));
            }
            return output;
        }
    }
}
