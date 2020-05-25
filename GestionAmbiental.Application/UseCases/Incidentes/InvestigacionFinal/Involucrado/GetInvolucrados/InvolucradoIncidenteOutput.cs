using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.GetInvolucrados
{
    public class InvolucradoIncidenteOutput
    {
        public int Id_Involucrado_Incidente { get; set; }
        public int? Id_Investigacion { get; set; }
        public int? Id_Usuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string DocIdentidad { get; set; }
        public int? Edad { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Puesto_Trabajo { get; set; }
        public string Empresa { get; set; }
        public string Turno { get; set; }
        public string Tipo_Involucrado { get; set; }
        public string Estado { get; set; }
    }
}
