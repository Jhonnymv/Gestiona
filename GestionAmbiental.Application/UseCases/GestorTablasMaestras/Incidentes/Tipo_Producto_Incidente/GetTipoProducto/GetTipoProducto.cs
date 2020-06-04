using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente.GetTipoProducto
{
    public class GetTipoProducto : IGetTipoProducto
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTipoProducto(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TipoProductoOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetTipo_Producto_Incidente(id);
            var output = new TipoProductoOutput(data);
            return output;
        }
    }
}
