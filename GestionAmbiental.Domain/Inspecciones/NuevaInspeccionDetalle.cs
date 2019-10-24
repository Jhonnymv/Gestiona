using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class NuevaInspeccionDetalle
    {
        public int Id_Inspeccion_Cabecera { get; set; }
        public string Descripcion_Condicion { get; set; }
        public int Id_Tipo_Condicion { get; set; }
        public string Evidencia_Condicion { get; set; }
        public string Nivel_Riesgo_Ambiental { get; set; }
        public string Descripcion_Accion { get; set; }
        public string Tipo_Accion { get; set; }
        public int Id_Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime Fecha_Programada { get; set; }
        public string Evidencia_Inspeccion { get; set; }
        public string Comentario_Inspeccion { get; set; }
        public DateTime Fecha_Cierre { get; set; }
        public DateTime Fecha_Verificacion_Inspector { get; set; }
        public string Comentario_Inspector { get; set; }
        public DateTime Fecha_Verificacion_MA { get; set; }
        public string Comentario_MA { get; set; }
        public string Estado { get; set; }

        public NuevaInspeccionDetalle(
            int id_Inspeccion_Cabecera, string descripcion_Condicion, int id_Tipo_Condicion, 
            string evidencia_Condicion, string nivel_Riesgo_Ambiental, string descripcion_Accion, 
            int id_Responsable_Marcobre, string responsable_Contratista, DateTime fecha_Programada, string evidencia_Inspeccion)
        {
            Id_Inspeccion_Cabecera = id_Inspeccion_Cabecera;
            Descripcion_Condicion = descripcion_Condicion;
            Id_Tipo_Condicion = id_Tipo_Condicion;
            Evidencia_Condicion = evidencia_Condicion;
            Nivel_Riesgo_Ambiental = nivel_Riesgo_Ambiental;
            Descripcion_Accion = descripcion_Accion;
            Id_Responsable_Marcobre = id_Responsable_Marcobre;
            Responsable_Contratista = responsable_Contratista;
            Fecha_Programada = fecha_Programada;
            Evidencia_Inspeccion = evidencia_Inspeccion;
        }
        public NuevaInspeccionDetalle() { }
    }

}
