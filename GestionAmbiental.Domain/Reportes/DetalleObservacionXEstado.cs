using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Reportes
{
    public class DetalleObservacionXEstado
    {
        public int IdInspeccionCabecera { get; set; }
        public string IdInspeccionCabeceraEncriptado { get; set; }
        public string Codigo { get; set; }
        public int IdInspeccionDetalle { get; set; }
        public string TipoObservacion { get; set; }
        public string DescripcionCondicion { get; set; }
        public string TipoCondicion { get; set; }
        public string EvidenciaCondicion { get; set; }
        public string NivelRiesgo { get; set; }
        public string DescripcionAccion { get; set; }
        public string ResponsableMarcobre { get; set; }
        public string ResponsableContratista { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string EvidenciaImplementacion { get; set; }
        public string ComentarioImplementacion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string Asesor { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public string ComentarioVerificacion { get; set; }
        public string Estado { get; set; }
    }
}
