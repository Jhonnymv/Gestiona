using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.GetTiposAgua
{
    public class GetTiposAgua : IGetTiposAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposAguaOutput> Execute(string filtro, int pagina, int rpp)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetTipo_Agua(filtro);

            var outputs = new List<TipoAguaOutput>();

            foreach (var item in data)
            {
                var temp = new TipoAguaOutput();
                temp.Id_Tipo_Agua = item.Id_Tipo_Agua.Value;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            outputs = outputs.OrderBy(x => x.Descripcion).ToList();

            var output = new TiposAguaOutput
            {
                NTipos = outputs.Count,
                Tipos = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
