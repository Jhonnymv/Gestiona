using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetAccionesIncidente
{
    public class AccioneIncidenteOutput
    {
        public int Id_Accion_Correctiva_Incidente { get; set; }
        public int Id_Reporte_Preliminar { get; set; }
        public int? Id_Usuario { get; set; }
        public string Responsable_Marcobre { get; set; }
        public string Accion_Inmediata { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; }
        public string Estado { get; set; }
    }
}
