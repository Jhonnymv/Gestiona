using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    public class Accion_Correctiva_Incidente
    {
        public int Id_Accion_Correctiva_Incidente { get; set; }
        public int Id_Investigacion_Final { get; set; }
        public int Id_Reporte_Preliminar { get; set; }
        public string Accion_Inmediata { get; set; }
        public int? Id_Usuario { get; set; }
        public string Responsable { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; }
        public DateTime? Fecha_Verificacion { get; set; }
        public string Comentarios_Verificacion { get; set; }
        public string Estado { get; set; }
    }
}
