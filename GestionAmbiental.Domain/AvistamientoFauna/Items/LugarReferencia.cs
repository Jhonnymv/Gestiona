using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.AvistamientoFauna.Items
{
    public class LugarReferencia
    {
        public int IdLugarReferencia { get; set; }
        public string Descripcion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Estado { get; set; }
    }
}
