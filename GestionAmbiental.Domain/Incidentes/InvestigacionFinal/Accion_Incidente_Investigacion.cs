using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Incidentes.InvestigacionFinal
{
    public class Accion_Incidente_Investigacion
    {
        public int Id_Accion_Incidente_Investigacion { get; set; }
        public int? Id_Investigacion_Final { get; set; }
        public string Tipo_Accion { get; set; }
        public string Accion_Inmediata { get; set; }
        public int? Id_Usuario { get; set; }
        public int? Id_Usuario2 { get; set; }
        public string Responsable { get; set; }
        public DateTime? Fecha { get; set; }
        public string Evidencia { get; set; }
        public string Estatus { get; set; }
        public DateTime? Fecha_Implementacion { get; set; }
        public string Comentarios_Implementacion { get; set; }
        public DateTime? Fecha_Verificacion { get; set; }
        public string Comentarios_Verificacion { get; set; }
        public string Estado { get; set; }
    }
}
