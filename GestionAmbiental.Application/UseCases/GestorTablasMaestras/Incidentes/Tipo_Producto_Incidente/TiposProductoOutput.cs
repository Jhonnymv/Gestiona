using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Producto_Incidente
{
    public class TiposProductoOutput
    {
        public List<TipoProductoOutput> Tipos { get; set; }
        public int NTipos { get; set; }
    }
}
