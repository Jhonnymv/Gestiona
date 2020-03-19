using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRSDescarga
{
    public class ConsultarManifiestoRSDescarga : IConsultarManifiestoRSDescarga
    {
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;

        public ConsultarManifiestoRSDescarga(IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository)
        {
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
        }

        public async Task<List<ManifiestoRSOutput>> Execute(string FechaInicio, string FechaFin)
        {
            var fechaInicio = DateTime.Parse(FechaInicio);
            var fechaFin = DateTime.Parse(FechaFin);
            var manifiestos = await _getManifiestosRSReadOnlyRepository.GetManifiestosRSDescarga(fechaInicio, fechaFin);
            var output = new List<ManifiestoRSOutput>();
            foreach(var item in manifiestos)
            {
                output.Add(new ManifiestoRSOutput(item));
            }
            return output;
        }
    }
}
