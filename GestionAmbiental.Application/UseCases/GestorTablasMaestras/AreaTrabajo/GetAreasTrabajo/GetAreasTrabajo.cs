using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.GetAreasTrabajo
{
    public class GetAreasTrabajo : IGetAreasTrabajo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetAreasTrabajo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<AreasTrabajoOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetAreaTrabajo(filtro);

            var outputs = new List<AreaTrabajoOutput>();
            foreach (var item in input)
            {
                var temp = new AreaTrabajoOutput();
                temp.IdAreaTrabajo = item.IdAreaTrabajo;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new AreasTrabajoOutput
            {
                NAreasTrabajo = input.Count,
                AreasTrabajo = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
