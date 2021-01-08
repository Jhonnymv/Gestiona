using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos
{
    public class RevisionAprobacionInputOutput
    {
        public int IdSGARevisionDocumento { get; set; }
        public int IdSGATipoRevision { get; set; }
        public string SGATipoRevision { get; set; }
        public int IdSGADocumento { get; set; }
        public string Comentario { get; set; }
        public string Estado { get; set; }
        public int Usuario { get; set; }
        public string UsuarioNombre { get; set; }
        public int Orden { get; set; }
        public string Fecha { get; set; }
        public DateTime? FechaLimite { get; set; }
        public DateTime? FechaRevision { get; set; }
    }
}
