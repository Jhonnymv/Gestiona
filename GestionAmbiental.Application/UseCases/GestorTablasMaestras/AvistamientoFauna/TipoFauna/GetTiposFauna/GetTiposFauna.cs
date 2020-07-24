using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.GetTiposFauna
{
    public class GetTiposFauna : IGetTiposFauna
    {
        public readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposFaunaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var tiposFauna = await _gestorTablasMaestrasReadOnlyRepository.GetTiposFauna(filtro);

            var outputs = new List<TipoFaunaOutput>();
            foreach (var item in tiposFauna)
            {
                var temp = new TipoFaunaOutput();
                temp.IdTipoFauna = item.IdTipoFauna;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new TiposFaunaOutput
            {
                NTiposFauna = tiposFauna.Count,
                TiposFauna = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }

        public async Task<TiposFaunaOutput> Execute()
        {
            var tiposFauna = await _gestorTablasMaestrasReadOnlyRepository.GetTiposFauna(null);

            var outputs = new List<TipoFaunaOutput>();
            foreach (var item in tiposFauna)
            {
                var temp = new TipoFaunaOutput();
                temp.IdTipoFauna = item.IdTipoFauna;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new TiposFaunaOutput
            {
                NTiposFauna = tiposFauna.Count,
                TiposFauna = outputs
            };

            return output;
        }
    }
}
