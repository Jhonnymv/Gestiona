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
        public string DescripcionActoCondicion { get; set; }
        public string DescripcionCondicion { get; set; }
        public int? IdTipoCondicion { get; set; }
        public string EvidenciaCondicion { get; set; }
        public string? NivelRiesgo { get; set; }
        public string DescripcionAccion { get; set; }
        public int IdResponsableMarcobre { get; set; }
        public string ResponsableContratista { get; set; }
        public string CorreoContratista { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public string EvidenciaImplementacion { get; set; }
        public string ComentarioImplementacion { get; set; }
        public string ComentarioVerificacion { get; set; }
        public string Estado { get; set; }
        public string Accion { get; set; }
        
        public NuevaInspeccionDetalle() { }
    }

}
