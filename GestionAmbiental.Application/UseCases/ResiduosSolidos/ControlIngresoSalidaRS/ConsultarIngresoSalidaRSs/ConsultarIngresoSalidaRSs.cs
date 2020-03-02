using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlIngresoSalidaRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRSs
{
    public class ConsultarIngresoSalidaRSs : IConsultarIngresoSalidaRSs
    {
        private readonly IGetIngresoSalidaRSReadOnlyRepository _getIngresoSalidaRSReadOnlyRepository;

        public ConsultarIngresoSalidaRSs(IGetIngresoSalidaRSReadOnlyRepository getIngresoSalidaRSReadOnlyRepository)
        {
            _getIngresoSalidaRSReadOnlyRepository = getIngresoSalidaRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getIngresoSalidaRSReadOnlyRepository));
        }
        public async Task<IngresoSalidaRSsOutput> Execute(int acr_cmrs, int ingreso_salida, int tipo, string fecha, int empresa, int asesorMA, int pagina, int rpp)
        {
            DateTime? fechadt = null;
            if (fecha != null)
                fechadt = Convert.ToDateTime(fecha);

            var data = await _getIngresoSalidaRSReadOnlyRepository.GetIngreso_Salida_RSs(acr_cmrs, ingreso_salida, tipo, fechadt, empresa, asesorMA);

            var output = new IngresoSalidaRSsOutput
            {
                Registros = new List<IngresoSalidaRSOutput>(),
                NRegistros = data.Count
            };

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                output.Registros.Add(new IngresoSalidaRSOutput(item));
            }
            return output;
        }
    }
}
