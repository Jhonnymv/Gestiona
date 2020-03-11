using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestosRSCierre
{
    public class ConsultarManifiestosRSCierre : IConsultarManifiestosRSCierre
    {
        private readonly IGetManifiestosRSReadOnlyRepository _getManifiestosRSReadOnlyRepository;
        public ConsultarManifiestosRSCierre(IGetManifiestosRSReadOnlyRepository getManifiestosRSReadOnlyRepository)
        {
            _getManifiestosRSReadOnlyRepository = getManifiestosRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getManifiestosRSReadOnlyRepository));
        }
        public async Task<ManifiestosRSOutput> Execute(string nManifiesto, string fechaEmision, string observacion, int residuoSolido, int eors_salida, int eors_llegada, int asesorMAP, int pagina, int rpp)
        {
            DateTime? fechadt = null;
            if (fechaEmision != null)
                fechadt = Convert.ToDateTime(fechaEmision);

            var data = await _getManifiestosRSReadOnlyRepository
                .GetManifiestosRS(nManifiesto, fechadt, observacion, residuoSolido, eors_salida, eors_llegada, asesorMAP);
            data = data.Where(x => x.Estado.Equals(EstadosResiduosSolidos.Registrado.ToString()) || x.Estado.Equals(EstadosResiduosSolidos.Cerrado.ToString())).OrderByDescending(x=>x.Item).ToList();
            var output = new ManifiestosRSOutput
            {
                manifiestosRS = new List<ManifiestoRSOutput>(),
                nManifiesto = data.Count
            };

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                output.manifiestosRS.Add(new ManifiestoRSOutput(item));
            }
            return output;
        }
    }
}
