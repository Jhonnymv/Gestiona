using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.GetEstadosFauna
{
    public class GetEstadosFauna : IGetEstadosFauna
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEstadosFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<EstadosFaunaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var input = await _gestorTablasMaestrasReadOnlyRepository.GetEstadoFauna(filtro);

            var outputs = new List<EstadoFaunaOutput>();
            foreach (var item in input)
            {
                var temp = new EstadoFaunaOutput();
                temp.IdEstadoFauna = item.IdEstadoFauna;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new EstadosFaunaOutput
            {
                NEstadosFauna = input.Count,
                EstadosFauna = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
