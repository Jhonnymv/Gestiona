using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Reportes
{
    public class DetalleObservacionXEstado
    {
        public int Id_Inspeccion_Cabecera { get; set; }
        public string Codigo { get; set; }
        public int Id_Inspeccion_Detalle { get; set; }
        public string Tipo_Observacion { get; set; }
        public string Descripcion_Condicion { get; set; }
        public string Tipo_Condicion { get; set; }
        public string Evidencia_Condicion { get; set; }
        public string Nivel_Riesgo { get; set; }
        public string Descripcion_Accion { get; set; }
        public string Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime Fecha_Programada { get; set; }
        public string Evidencia_Inspeccion { get; set; }
        public string Comentario_Inspeccion { get; set; }
        public DateTime? Fecha_Cierre { get; set; }
        public string Asesor_MAP_SSO { get; set; }
        public DateTime? Fecha_Verificacion { get; set; }
        public string Comentario_Verificacion { get; set; }
        public string Estado { get; set; }
    }
}
