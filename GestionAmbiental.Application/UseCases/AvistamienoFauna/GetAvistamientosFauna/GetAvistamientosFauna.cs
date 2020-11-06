using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Application.UseCases.Reportes;
using GestionAmbiental.Domain.Enum;
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

        public async Task<AvistamientosFaunaOutput> Execute(string fecha, int tipo_fauna, int estado_fauna, int lugar_referencia, int rpp, int pagina)
        {
            DateTime? fechaFiltro = null;
            if (fecha != null)
                fechaFiltro = Convert.ToDateTime(fecha);
            var data = await _getAvistamientoFaunaReadOnlyRepository.GetAvistamientosFauna(fechaFiltro,tipo_fauna,estado_fauna,lugar_referencia);
            var tbds = data.Where(x => x.Codigo.Equals("TBD")).OrderByDescending(x => x.FechaAvistamiento).ToList();
            data = data.Where(x => !x.Codigo.Equals("TBD")).OrderByDescending(x => Int32.Parse(x.Codigo)).ToList();
            data.InsertRange(0,tbds);
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

        public async Task<AvistamientosFaunaOutput> Execute(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, string fechaInicio, string fechaFin)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (fechaInicio != null)
                fInicio = Convert.ToDateTime(fechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (fechaFin != null)
                fFinal = Convert.ToDateTime(fechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;

            //Obtener valores
            var data = await _getAvistamientoFaunaReadOnlyRepository.GetAvistamientosFauna(nombre_comun, tipo_avistamiento, estado_fauna, lugar_referencia, fInicio, fFinal);
            data = data.Where(x => !x.Codigo.Equals("TBD")).ToList();
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new AvistamientosFaunaOutput();
            output.Avistamientos = new List<AvistamientoFaunaOutput>();
            output.NAvistamientos = data.Count;

            foreach (var item in data)
            {
                var outputItem = new AvistamientoFaunaOutput(item);
                output.Avistamientos.Add(outputItem);
            }

            return output;
        }
    }
}
