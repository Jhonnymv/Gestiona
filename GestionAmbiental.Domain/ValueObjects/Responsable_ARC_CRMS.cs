using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.ValueObjects
{
    public class Responsable_ARC_CRMS
    {
        public int Id_Responsable_ACR_CRMS { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Estado { get; set; }
        public Responsable_ARC_CRMS()
        {
        }
        public Responsable_ARC_CRMS(int id_Responsable_ACR_CRMS, string nombres, string apellidos, string estado)
        {
            Id_Responsable_ACR_CRMS = id_Responsable_ACR_CRMS;
            Nombres = nombres;
            Apellidos = apellidos;
            Estado = estado;
        }
    }
}
