using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente.GetProductosIncidente
{
    public class GetProductosIncidente : IGetProductosIncidente
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetProductosIncidente(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ProductosIncidenteOutput> Execute(string filtro, int pagina, int rpp)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetProducto_Incidente(filtro);

            var outputs = new List<ProductoIncidenteOutput>();

            foreach (var item in data)
            {
                var temp = new ProductoIncidenteOutput();
                temp.Id_Producto_Incidente = item.Id_Producto_Incidente.Value;
                temp.Descripcion = item.Descripcion;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            outputs = outputs.OrderBy(x => x.Descripcion).ToList();

            var output = new ProductosIncidenteOutput
            {
                NPRoductos = outputs.Count,
                Productos = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
