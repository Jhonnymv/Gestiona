using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.GetTiposInspeccion
{
    internal class GetTiposInspeccion : IGetTiposInspeccion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposInspeccion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposInspeccionOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetTiposInspeccion(filtro);

            var outputs = new List<TipoInspeccionOutput>();
            foreach (var item in input)
            {
                var temp = new TipoInspeccionOutput();
                temp.IdTipoInspeccion = item.Id_Tipo_Inspeccion;
                temp.Descripcion = item.Descripcion;
                outputs.Add(temp);
            }
            var output = new TiposInspeccionOutput
            {
                NTiposInspeccion = input.Count,
                TiposInspeccion = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }

        public async Task<List<TipoInspeccionOutput>> Execute()
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetTiposInspeccion(null);
            var outputs = new List<TipoInspeccionOutput>();
            foreach (var item in input)
            {
                var temp = new TipoInspeccionOutput();
                temp.IdTipoInspeccion = item.Id_Tipo_Inspeccion;
                temp.Descripcion = item.Descripcion;
                outputs.Add(temp);
            }

            return outputs;
        }
    }
}
