using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class NuevaInspeccionDetalle
    {
        public int IdInspeccionDetalle { get; set; }
        public int IdInspeccionCabecera { get; set; }
        public string TipoInspeccion { get; set; }
        public int IdAsesorMASeguridad { get; set; }
        public string DescripcionCondicion { get; set; }
        public int IdTipoCondicion { get; set; }
        public string EvidenciaCondicion { get; set; }
        public string NivelRiesgo { get; set; }
        public string DescripcionAccion { get; set; }
        public int IdResponsableMarcobre { get; set; }
        public string ResponsableContratista { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string EvidenciaImplementacion { get; set; }
        public string ComentarioImplementacion { get; set; }
        public string ComentarioVerificacion { get; set; }
        public string Estado { get; set; }
        public string Accion { get; set; }

        public NuevaInspeccionDetalle(
            int id_Inspeccion_Cabecera, string descripcion_Condicion, int id_Tipo_Condicion, 
            string evidencia_Condicion, string nivel_Riesgo_Ambiental, string descripcion_Accion, 
            int id_Responsable_Marcobre, string responsable_Contratista, DateTime fecha_Programada)
        {
            IdInspeccionCabecera = id_Inspeccion_Cabecera;
            DescripcionCondicion = descripcion_Condicion;
            IdTipoCondicion = id_Tipo_Condicion;
            EvidenciaCondicion = evidencia_Condicion;
            NivelRiesgo = nivel_Riesgo_Ambiental;
            DescripcionAccion = descripcion_Accion;
            IdResponsableMarcobre = id_Responsable_Marcobre;
            ResponsableContratista = responsable_Contratista;
            FechaProgramada = fecha_Programada;
        }
        public NuevaInspeccionDetalle() { }
    }

}
