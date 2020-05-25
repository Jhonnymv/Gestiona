using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.ReportePreliminar
{
    public class Involucrado_Incidente_Reporte
    {
        public int Id_Involucrado_Incidente_Reporte { get; set; }
        public int? Id_Reporte_Preliminar { get; set; }
        public int? Id_Usuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Puesto_Trabajo { get; set; }
        public string Turno { get; set; }
        public string Tipo_Involucrado { get; set; }
        public string Estado { get; set; }
    }
}
