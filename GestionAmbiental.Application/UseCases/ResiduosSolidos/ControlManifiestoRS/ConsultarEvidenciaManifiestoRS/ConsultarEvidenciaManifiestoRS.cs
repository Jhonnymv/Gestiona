using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarEvidenciaManifiestoRS
{
    public class ConsultarEvidenciaManifiestoRS : IConsultarEvidenciaManifiestoRS
    {
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;

        public ConsultarEvidenciaManifiestoRS(IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository)
        {
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
        }

        public async Task<string[]> Execute(int IdManifiesto, string Tipo)
        {
            var evidencias = await _getManifiestosRSReadOnlyRepository.GetEvidenciaManifiesto(IdManifiesto,Tipo);
            var outputTemp = new List<string>();
            string[] output = { };
            if (evidencias.Length > 0)
            {
                var temp = JArray.Parse(evidencias);
                foreach (var item in temp)
                {
                    outputTemp.Add((string)item);
                }
                output = outputTemp.ToArray();
            }
            return output;
        }
    }
}
