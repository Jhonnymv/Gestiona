using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class Documento
    {
        public int IdSGADocumento { get; set; }
        public CategoriaTipo SGACategoriaTipo { get; set; }
        public string DocCodigo { get; set; }
        public string DocNombre { get; set; }
        public double DocVersion { get; set; }
        public Documento DocPredecesor { get; set; }
        public Documento DocPrincipal { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public Condicion Condicion { get; set; }
        public string Comentario { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public string DocRuta { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Estado { get; set; }

        public List<RevisionDocumento> Revisiones { get; set; }
        public List<RevisionDocumento> Aprobaciones { get; set; }
    }
}
