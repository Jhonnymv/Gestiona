using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombresComunes
{
    public class GetNombresComunes : IGetNombresComunes
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetNombresComunes(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<NombresComunesOutput> Execute(string filtro, int pagina, int rpp)
        {
            var nombresComunes = await _gestorTablasMaestrasReadOnlyRepository.GetNombreComun(filtro);

            var outputs = new List<NombreComunOutput>();
            foreach (var item in nombresComunes)
            {
                var temp = new NombreComunOutput();
                temp.IdNombreComunFauna = item.IdNombreComunFauna;
                temp.IdTipoFauna = item.TipoFauna.IdTipoFauna;
                temp.TipoFauna = item.TipoFauna.Descripcion;
                temp.Descripcion = item.Descripcion;
                temp.Foto = item.Foto;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new NombresComunesOutput
            {
                NNombresComunes = nombresComunes.Count,
                NombresComunes = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
