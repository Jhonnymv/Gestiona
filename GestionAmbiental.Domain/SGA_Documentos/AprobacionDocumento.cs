using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class AprobacionDocumento
    {
        public int IdSGARevisionDocumento { get; set; }
        public int IdSGADocumento { get; set; }
        public int IdUsuario { get; set; }
        public int Orden { get; set; }
        public DateTime? FechaLimite { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string Estado { get; set; }
    }
}
