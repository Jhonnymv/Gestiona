using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Verificacion.ConsultarRAC
{
    public class ConsultarRAC : IConsultarRAC
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;

        public ConsultarRAC(IGetRACReadOnlyRepository getRACReadOnlyRepository)
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
        }

        public async Task<RACOutput> Execute(int Id_RAC)
        {
            var reporte = await _getRACReadOnlyRepository.GetRAC(Id_RAC);
            return new RACOutput(reporte);
        }
    }
}
