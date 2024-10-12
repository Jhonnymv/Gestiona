using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class MiembroComiteInspeccion
    {
        public int Id_Miembro_Comite { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Estado { get; set; }

        public MiembroComiteInspeccion()
        {
        }
    }
}
