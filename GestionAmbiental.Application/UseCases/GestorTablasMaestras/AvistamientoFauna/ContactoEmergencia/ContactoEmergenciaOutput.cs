using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.ContactoEmergencia
{
    public class ContactoEmergenciaOutput
    {
        public int IdContactoEmergencia { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Estado { get; set; }

        public ContactoEmergenciaOutput()
        {
        }
        public ContactoEmergenciaOutput(Domain.AvistamientoFauna.Items.ContactoEmergencia contactoEmergencia)
        {
            IdContactoEmergencia = contactoEmergencia.IdContactoEmergencia;
            Nombres = contactoEmergencia.Nombres;
            Apellidos = contactoEmergencia.Apellidos;
            Telefono = contactoEmergencia.Telefono;
            Estado = contactoEmergencia.Estado;
        }
    }
}
