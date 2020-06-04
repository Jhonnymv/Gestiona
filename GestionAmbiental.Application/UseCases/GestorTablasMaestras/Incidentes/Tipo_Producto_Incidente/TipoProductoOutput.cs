using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente
{
    public class TipoProductoOutput
    {
        public int Id_Tipo_Producto_Incidente { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public TipoProductoOutput()
        {
        }
        public TipoProductoOutput(Domain.Incidentes.Items.Tipo_Producto_Incidente tipo)
        {
            Id_Tipo_Producto_Incidente = tipo.Id_Tipo_Producto_Incidente.Value;
            Descripcion = tipo.Descripcion;
            Estado = tipo.Estado;
        }
    }
}
