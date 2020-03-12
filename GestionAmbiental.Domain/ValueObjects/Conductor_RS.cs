using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Conductor_RS
    {
        public int Id_Conductor_RS { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NLicencia { get; set; }
        public string Estado { get; set; }

        public Conductor_RS(int id_Conductor_RS, string nombres, string apellidos, string nLicencia, string estado)
        {
            Id_Conductor_RS = id_Conductor_RS;
            Nombres = nombres;
            Apellidos = apellidos;
            NLicencia = nLicencia;
            Estado = estado;
        }

        public Conductor_RS()
        {
        }
    }
}
