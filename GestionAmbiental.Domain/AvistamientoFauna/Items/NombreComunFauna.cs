using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.AvistamientoFauna.Items
{
    public class NombreComunFauna
    {
        public int IdNombreComunFauna { get; set; }
        public string Descripcion { get; set; }
        public TipoFauna TipoFauna { get; set; }
        public string Foto { get; set; }
        public string Estado { get; set; }

        public NombreComunFauna()
        {
        }

    }
}
