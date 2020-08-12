using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.GetTiposAvistamiento
{
    public class GetTiposAvistamiento : IGetTiposAvistamiento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposAvistamiento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposAvistamientoOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetTipoAvistamiento(filtro);

            var outputs = new List<TipoAvistamientoOutput>();
            foreach (var item in input)
            {
                var temp = new TipoAvistamientoOutput();
                temp.IdTipoAvistamiento = item.IdTipoAvistamiento;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new TiposAvistamientoOutput
            {
                NTiposAvistamiento = input.Count,
                TiposAvistamiento = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
