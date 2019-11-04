using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class InspeccionDetalle
    {
        public int Id_Inspeccion_Cabecera { get; set; }
        public int Id_Inspeccion_Detalle { get; set; }
        public string Descripcion_Condicion { get; set; }
        public Tipo_Condicion Tipo_Condicion { get; set; }
        public string Evidencia_Condicion { get; set; }
        public string Nivel_Riesgo_Ambiental { get; set; }
        public string Descripcion_Accion { get; set; }
        public Usuario Responsable_Marcobre { get; set; }
        public string Responsable_Contratista { get; set; }
        public DateTime Fecha_Programada { get; set; }
        public string Evidencia_Inspeccion { get; set; }
        public string Comentario_Inspeccion { get; set; }
        public DateTime? Fecha_Cierre { get; set; }
        public DateTime? Fecha_Verificacion_Inspector { get; set; }
        public string Comentario_Inspector { get; set; }
        public DateTime? Fecha_Verificacion_MA { get; set; }
        public string Comentario_MA { get; set; }
        public string Estado { get; set; }

        public InspeccionDetalle(
            int id_Inspeccion_Cabecera, int id_Inspeccion_Detalle, string descripcion_Condicion, 
            Tipo_Condicion tipo_Condicion, string evidencia_Condicion, string nivel_Riesgo_Ambiental, 
            string descripcion_Accion, Usuario responsable_Marcobre, string responsable_Contratista, DateTime fecha_Programada)
        {
            Id_Inspeccion_Cabecera = id_Inspeccion_Cabecera;
            Id_Inspeccion_Detalle = id_Inspeccion_Detalle;
            Descripcion_Condicion = descripcion_Condicion;
            Tipo_Condicion = tipo_Condicion;
            Evidencia_Condicion = evidencia_Condicion;
            Nivel_Riesgo_Ambiental = nivel_Riesgo_Ambiental;
            Descripcion_Accion = descripcion_Accion;
            Responsable_Marcobre = responsable_Marcobre;
            Responsable_Contratista = responsable_Contratista;
            Fecha_Programada = fecha_Programada;
        }

        public InspeccionDetalle(
            int id_Inspeccion_Cabecera,
            int id_Inspeccion_Detalle,
            string descripcion_Condicion,
            Tipo_Condicion tipo_Condicion, 
            string evidencia_Condicion, 
            string nivel_Riesgo_Ambiental,
            string descripcion_Accion,
            Usuario responsable_Marcobre,
            string responsable_Contratista,
            DateTime fecha_Programada, 
            string evidencia_Inspeccion, 
            string comentario_Inspeccion,
            DateTime? fecha_Cierre,
            DateTime? fecha_Verificacion_Inspector,
            string comentario_Inspector,
            DateTime? fecha_Verificacion_MA,
            string comentario_MA,
            string estado)
        {
            Id_Inspeccion_Cabecera = id_Inspeccion_Cabecera;
            Id_Inspeccion_Detalle = id_Inspeccion_Detalle;
            Descripcion_Condicion = descripcion_Condicion;
            Tipo_Condicion = tipo_Condicion;
            Evidencia_Condicion = evidencia_Condicion;
            Nivel_Riesgo_Ambiental = nivel_Riesgo_Ambiental;
            Descripcion_Accion = descripcion_Accion;
            Responsable_Marcobre = responsable_Marcobre;
            Responsable_Contratista = responsable_Contratista;
            Fecha_Programada = fecha_Programada;
            Evidencia_Inspeccion = evidencia_Inspeccion;
            Comentario_Inspeccion = comentario_Inspeccion;
            Fecha_Cierre = fecha_Cierre;
            Fecha_Verificacion_Inspector = fecha_Verificacion_Inspector;
            Comentario_Inspector = comentario_Inspector;
            Fecha_Verificacion_MA = fecha_Verificacion_MA;
            Comentario_MA = comentario_MA;
            Estado = estado;
        }
    }
}
