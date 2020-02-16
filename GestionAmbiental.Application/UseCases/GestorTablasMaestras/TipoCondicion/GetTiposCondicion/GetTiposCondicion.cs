using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.GetTiposCondicion
{
    public class GetTiposCondicion : IGetTiposCondicion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposCondicion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposCondicionOutput> Execute(string filtro, int pagina, int rpp)
        {
            var tiposCondicion = await _gestorTablasMaestrasReadOnlyRepository.GetTiposCondicion(filtro);

            var tiposCondicionOutputs = new List<TipoCondicionOutput>();
            foreach (var item in tiposCondicion)
            { 
                var temp = new TipoCondicionOutput();
                temp.IdTipoCondicion = item.IdTipoCondicion;
                temp.Descripcion = item.Descripcion;
                temp.Grupo = item.Grupo;
                tiposCondicionOutputs.Add(temp);
            }
            tiposCondicionOutputs = tiposCondicionOutputs.OrderBy(x => x.Descripcion).ToList();
            var output = new TiposCondicionOutput
            {
                nTiposCondicion = tiposCondicionOutputs.Count,
                TiposCondicion = tiposCondicionOutputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };
            

            return output;
        }
    }
}
