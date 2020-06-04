using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Producto_Incidente
{
    public class ProductosIncidenteOutput
    {
        public List<ProductoIncidenteOutput> Productos { get; set; }
        public int NPRoductos { get; set; }
    }
}
