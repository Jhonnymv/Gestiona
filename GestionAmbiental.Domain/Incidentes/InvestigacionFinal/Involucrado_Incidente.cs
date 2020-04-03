using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    public class Involucrado_Incidente
    {
        public int Id_Involucrado_Incidente { get; set; }
        public int Id_Reporte_Preliminar { get; set; }
        public int Id_Investigacion_Final { get; set; }
        public int? Id_Usuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Doc_Identidad { get; set; }
        public int? Edad { get; set; }
        public string Puesto_Trabajo { get; set; }
        public string Empresa { get; set; }
        public string Turno { get; set; }
        public string Descripcion { get; set; }
        public string Tipo_Involucrado { get; set; }
        public string Estado { get; set; }
    }
}
