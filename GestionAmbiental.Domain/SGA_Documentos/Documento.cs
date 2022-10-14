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
        public string DocVersion { get; set; }
        public bool Principal { get; set; }
        public Documento DocPredecesor { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public Condicion Condicion { get; set; }
        public string Comentario { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public string PdfRuta { get; set; }
        public string EditableRuta { get; set; }
        public string? ElaboradoPorString { get; set; }
        public Usuario? Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string? Acceso { get; set; }
        public string Estado { get; set; }
        public List<RevisionDocumento> Revisiones { get; set; }
        public List<RevisionDocumento> Aprobaciones { get; set; }
        public List<Documento> Formatos { get; set; }
    }
}
