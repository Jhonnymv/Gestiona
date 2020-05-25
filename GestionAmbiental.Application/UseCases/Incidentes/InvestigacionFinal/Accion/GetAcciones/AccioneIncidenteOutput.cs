using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones
{
    public class AccioneIncidenteOutput
    {
        public int Id_Accion_Correctiva_Incidente { get; set; }
        public int? Id_Investigacion_Final { get; set; }
        public string Tipo_Accion { get; set; }
        public string Accion_Inmediata { get; set; }
        public int? Id_Usuario { get; set; }
        public string Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public string Evidencia { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; }
        public DateTime? Fecha_Implementacion { get; set; }
        public string Comentarios_Implementacion { get; set; }
        public DateTime? Fecha_Verificacion { get; set; }
        public string Comentarios_Verificacion { get; set; }
        public string Estado { get; set; }
        public bool Permiso_Implementacion { get; set; }
        public bool Permiso_Verificacion { get; set; }
    }
}
