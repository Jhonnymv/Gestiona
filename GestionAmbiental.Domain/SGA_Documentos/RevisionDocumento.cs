using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class RevisionDocumento
    {
        public int IdSGARevisionDocumento { get; set; }
        public TipoRevision SGATipoRevision { get; set; }
        public int IdSGADocumento { get; set; }
        public Usuario Usuario { get; set; }
        public int Orden { get; set; }
        public DateTime? FechaLimite { get; set; }
        public DateTime? FechaRevision { get; set; }
        public string Estado { get; set; }
    }
}
