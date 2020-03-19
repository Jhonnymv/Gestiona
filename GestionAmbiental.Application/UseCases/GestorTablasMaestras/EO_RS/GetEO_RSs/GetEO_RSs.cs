using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.GetEO_RSs
{
    public class GetEO_RSs : IGetEO_RSs
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEO_RSs(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<EO_RSsOutput> Execute(string filtro, int pagina, int rpp)
        {
            var eorss = await _gestorTablasMaestrasReadOnlyRepository.GetEO_RS(filtro);

            var outputs = new List<EO_RSOutput>();
            foreach (var item in eorss)
            {
                var temp = new EO_RSOutput();
                temp.Razon_Social = item.Razon_Social;
                temp.Registro = item.Registro;
                temp.Fecha_Vencimiento_Registro = item.Fecha_Vencimiento_Registro;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new EO_RSsOutput
            {
                NEO_RSs = eorss.Count,
                EO_RSs = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
