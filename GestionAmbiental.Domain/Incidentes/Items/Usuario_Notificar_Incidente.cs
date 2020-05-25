using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.Items
{
    public class Usuario_Notificar_Incidente
    {
        public int Id_Usuario_Notificar_Incidente { get; set; }
        public int Id_Rol { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Usuario_Red { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public Usuario_Notificar_Incidente() { }
    }
}
