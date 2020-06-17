using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetEvidenciaInvestigacion
{
    public class GetEvidenciasInvestigacion : IGetEvidenciaInvestigacion
    {
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;

        public GetEvidenciasInvestigacion(IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository)
        {
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
        }

        public async Task<string[]> Execute(int IdInvestigacion, string Tipo)
        {
            var evidencias = await _getInvestigacionFinalReadOnlyRepository.GetEvidenciaInvestigacion(IdInvestigacion, Tipo);
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
