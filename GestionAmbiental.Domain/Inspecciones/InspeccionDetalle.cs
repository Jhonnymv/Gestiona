using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class InspeccionDetalle
    {
        public int IdInspeccionCabecera { get; set; }
        public int IdInspeccionDetalle { get; set; }
        public string TipoObservacion { get; set; }
        public string DescripcionCondicion { get; set; }
        public TipoCondicion TipoCondicion { get; set; }
        public string EvidenciaCondicion { get; set; }
        public string NivelRiesgo { get; set; }
        public string DescripcionAccion { get; set; }
        public Usuario ResponsableMarcobre { get; set; }
        public Usuario? ResponsableMarcobre2 { get; set; }
        public string ResponsableContratista { get; set; }
        public string CorreoContratista { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public string EvidenciaImplementacion { get; set; }
        public string ComentarioImplementacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public Usuario Asesor { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public string ComentarioVerificacion { get; set; }
        public string Estado { get; set; }
        public string DescripcionActoCondicion { get; set; }
                
        public InspeccionDetalle(
            int id_Inspeccion_Cabecera,
            int id_Inspeccion_Detalle,
            string tipo_Observacion,
            string descripcion_Condicion, 
            TipoCondicion tipo_Condicion, 
            string evidencia_Condicion, 
            string nivel_Riesgo_Ambiental,
            string descripcion_Accion,
            Usuario responsable_Marcobre, 
            string responsable_Contratista, 
            DateTime fecha_Programada,
            string evidencia_Inspeccion, 
            string comentario_Inspeccion, 
            DateTime? fecha_Cierre, 
            Usuario responsable_MA, 
            DateTime? fecha_Verificacion_MA,
            string comentario_MA, 
            string estado)
        {
            IdInspeccionCabecera = id_Inspeccion_Cabecera;
            IdInspeccionDetalle = id_Inspeccion_Detalle;
            TipoObservacion = tipo_Observacion;
            DescripcionCondicion = descripcion_Condicion;
            TipoCondicion = tipo_Condicion;
            EvidenciaCondicion = evidencia_Condicion;
            NivelRiesgo = nivel_Riesgo_Ambiental;
            DescripcionAccion = descripcion_Accion;
            ResponsableMarcobre = responsable_Marcobre;
            ResponsableContratista = responsable_Contratista;
            FechaProgramada = fecha_Programada;
            EvidenciaImplementacion = evidencia_Inspeccion;
            ComentarioImplementacion = comentario_Inspeccion;
            FechaCierre = fecha_Cierre;
            Asesor = responsable_MA;
            FechaVerificacion = fecha_Verificacion_MA;
            ComentarioVerificacion = comentario_MA;
            Estado = estado;
        }

        public InspeccionDetalle()
        {
        }
    }
}
