using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.GetTiposProducto
{
    public class GetTiposProducto : IGetTiposProducto
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTiposProducto(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TiposProductoOutput> Execute(string filtro, int pagina, int rpp)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetTipo_Producto_Incidente(filtro);

            var outputs = new List<TipoProductoOutput>();

            foreach (var item in data)
            {
                var temp = new TipoProductoOutput();
                temp.Id_Tipo_Producto_Incidente = item.Id_Tipo_Producto_Incidente.Value;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            outputs = outputs.OrderBy(x => x.Descripcion).ToList();

            var output = new TiposProductoOutput
            {
                NTipos = outputs.Count,
                Tipos = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
