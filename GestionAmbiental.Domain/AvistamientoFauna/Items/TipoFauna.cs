using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.AvistamientoFauna.Items
{
    public class TipoFauna
    {
        public int IdTipoFauna { get; set; }
        public string Descripcion { get; set; }
        public string Foto { get; set; }
        public string Estado { get; set; }
    }
}
