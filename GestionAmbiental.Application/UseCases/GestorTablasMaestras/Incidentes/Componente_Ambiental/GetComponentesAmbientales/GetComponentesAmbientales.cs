using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.GetComponentesAmbientales
{
    public class GetComponentesAmbientales : IGetComponentesAmbientales
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetComponentesAmbientales(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ComponentesAmbientalesOutput> Execute(string filtro, int pagina, int rpp)
        {
            var componentes = await _gestorTablasMaestrasReadOnlyRepository.GetComponente_Ambientales(filtro);

            var outputs = new List<ComponenteAmbientalOutput>();

            foreach (var item in componentes)
            {
                var temp = new ComponenteAmbientalOutput();
                temp.Id_Componente_Ambiental = item.Id_Componente_Ambiental.Value;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            outputs = outputs.OrderBy(x => x.Descripcion).ToList();

            var output = new ComponentesAmbientalesOutput
            {
                NComponentes = outputs.Count,
                Componentes = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
