using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.AvistamientoFauna.Items
{
    public class TipoAvistamiento
    {
        public int IdTipoAvistamiento { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
