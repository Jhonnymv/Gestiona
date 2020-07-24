using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientosFauna
{
    public class GetAvistamientosFauna : IGetAvistamientosFauna
    {
        private readonly IGetAvistamientoFaunaReadOnlyRepository _getAvistamientoFaunaReadOnlyRepository;

        public GetAvistamientosFauna(IGetAvistamientoFaunaReadOnlyRepository getAvistamientoFaunaReadOnlyRepository)
        {
            _getAvistamientoFaunaReadOnlyRepository = getAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(getAvistamientoFaunaReadOnlyRepository));
        }

        public async Task<AvistamientosFaunaOutput> Execute(string fecha, int tipo_fauna, int estado_fauna, int rpp, int pagina)
        {
            DateTime? fechaFiltro = null;
            if (fecha != null)
                fechaFiltro = Convert.ToDateTime(fecha);
            var data = await _getAvistamientoFaunaReadOnlyRepository.GetAvistamientosFauna(fechaFiltro,tipo_fauna,estado_fauna);

            var output = new AvistamientosFaunaOutput();
            output.Avistamientos = new List<AvistamientoFaunaOutput>();
            output.NAvistamientos = data.Count;

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                var outputItem = new AvistamientoFaunaOutput(item);
                output.Avistamientos.Add(outputItem);
            }

            return output;

        }
    }
}
