using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.ReportePreliminar
{
    public class Accion_Incidente_Reporte
    {
        public int Id_Accion_Incidente_Reporte { get; set; }
        public int Id_Reporte_Preliminar { get; set; }
        public string Accion_Inmediata { get; set; }
        public int? Id_Usuario { get; set; }
        public string Responsable { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; }
        public string Estado { get; set; }
    }
}
