using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.GetTiposSuelo
{
    public class GetTiposSuelo : IGetTiposSuelo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposSuelo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposSueloOutput> Execute(string filtro, int pagina, int rpp)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetTipo_Suelo(filtro);

            var outputs = new List<TipoSueloOutput>();

            foreach (var item in data)
            {
                var temp = new TipoSueloOutput();
                temp.Id_Tipo_Suelo = item.Id_Tipo_Suelo.Value;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            outputs = outputs.OrderBy(x => x.Descripcion).ToList();

            var output = new TiposSueloOutput
            {
                NTipos = outputs.Count,
                Tipos = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
