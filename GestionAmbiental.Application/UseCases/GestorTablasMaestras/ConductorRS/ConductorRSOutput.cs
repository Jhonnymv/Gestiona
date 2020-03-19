using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS
{
    public class ConductorRSOutput
    {
        public int Id_Conductor_RS { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NLicencia { get; set; }
        public string Estado { get; set; }
        public ConductorRSOutput() { }
        public ConductorRSOutput(Domain.ValueObjects.Conductor_RS conductor)
        {
            Id_Conductor_RS = conductor.Id_Conductor_RS;
            Nombres = conductor.Nombres;
            Apellidos = conductor.Apellidos;
            NLicencia = conductor.NLicencia;
            Estado = conductor.Estado;
        }
    }
}
