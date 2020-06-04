using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente
{
    public class ProductoIncidenteOutput
    {
        public int Id_Producto_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public ProductoIncidenteOutput()
        {
        }
        public ProductoIncidenteOutput(Domain.Incidentes.Items.Producto_Incidente producto)
        {
            Id_Producto_Incidente = producto.Id_Producto_Incidente.Value;
            Descripcion = producto.Descripcion;
            Estado = producto.Estado;
        }
    }
}
