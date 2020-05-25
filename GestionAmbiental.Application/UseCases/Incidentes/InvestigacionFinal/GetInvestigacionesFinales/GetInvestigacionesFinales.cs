using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionesFinales
{
    public class GetInvestigacionesFinales : IGetInvestigacionesFinales
    {
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;

        public GetInvestigacionesFinales(IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository)
        {
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
        }

        public async Task<InvestigacionesFinalesOutput> Execute(string fecha, int id_Usuario, int id_Gerencia, int id_Asesor, int pagina, int rpp)
        {
            DateTime? fechaFiltro = null;
            if (fecha != null)
                fechaFiltro = Convert.ToDateTime(fecha);
            var data = await _getInvestigacionFinalReadOnlyRepository.GetInvestigacionesFinales(fechaFiltro, id_Usuario, id_Asesor, id_Gerencia);

            var output = new InvestigacionesFinalesOutput();
            output.Investigaciones = new List<InvestigacionFinalOutput>();
            output.NInvestigaciones = data.Count;

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                var outputItem = new InvestigacionFinalOutput(item);
                output.Investigaciones.Add(outputItem);
            }

            return output;
        }
    }
}
