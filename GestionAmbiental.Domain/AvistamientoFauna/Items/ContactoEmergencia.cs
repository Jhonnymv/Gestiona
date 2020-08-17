using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.AvistamientoFauna.Items
{
    public class ContactoEmergencia
    {
        public int IdContactoEmergencia { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }
    }
}
