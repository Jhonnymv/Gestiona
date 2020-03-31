using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlIngresoSalidaRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRSDescarga
{
    public class ConsultarIngresoSalidaRSDescarga : IConsultarIngresoSalidaRSDescarga
    {
        private readonly IGetIngresoSalidaRSReadOnlyRepository _getIngresoSalidaRSReadOnlyRepository;

        public ConsultarIngresoSalidaRSDescarga(IGetIngresoSalidaRSReadOnlyRepository getIngresoSalidaRSReadOnlyRepository)
        {
            _getIngresoSalidaRSReadOnlyRepository = getIngresoSalidaRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getIngresoSalidaRSReadOnlyRepository));
        }

        public async Task<List<IngresoSalidaRSOutput>> Execute(string FechaInicio, string FechaFin)
        {
            var fechaInicio = DateTime.Parse(FechaInicio);
            var fechaFin = DateTime.Parse(FechaFin);
            var ingresos_salidas = await _getIngresoSalidaRSReadOnlyRepository.GetIngreso_Salida_RSDescarga(fechaInicio, fechaFin);
            var output = new List<IngresoSalidaRSOutput>();
            foreach (var item in ingresos_salidas)
            {
                output.Add(new IngresoSalidaRSOutput(item));
            }
            return output;
        }
    }
}
