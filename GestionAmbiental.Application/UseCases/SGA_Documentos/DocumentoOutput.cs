using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos
{
    public class DocumentoOutput
    {
        public int IdSGADocumento { get; set; }
        public int IdSGATipoDocumento { get; set; }
        public string SGATipoDocumento { get; set; }
        public string DocCodigo { get; set; }
        public string DocNombre { get; set; }
        public double DocVersion { get; set; }
        public int? IdDocPredecesor { get; set; }
        public string DocPredecesorNombre { get; set; }
        public string DocPredecesorCodigo { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public int IdCondicion { get; set; }
        public string Condicion { get; set; }
        public string Comentario { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public string DocRuta { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Estado { get; set; }

        public DocumentoOutput(Documento documento)
        {
            IdSGADocumento = documento.IdSGADocumento;
            IdSGATipoDocumento = documento.SGATipoDocumento.IdSGATipoDocumento;
            SGATipoDocumento = documento.SGATipoDocumento.Descripcion;
            DocCodigo = documento.DocCodigo;
            DocNombre = documento.DocNombre;
            DocVersion = documento.DocVersion;
            if (documento.DocPredecesor != null)
            {
                IdDocPredecesor = documento.DocPredecesor.IdSGADocumento;
                DocPredecesorNombre = documento.DocPredecesor.DocNombre;
                DocPredecesorCodigo = documento.DocPredecesor.DocCodigo;
            }
            FechaAprobacion = documento.FechaAprobacion;
            IdCondicion = documento.Condicion.IdSGACondicion;
            Condicion = documento.Condicion.Descripcion;
            Comentario = documento.Comentario;
            FechaTentativa = documento.FechaTentativa;
            FechaActualizacion = documento.FechaActualizacion;
            Estado = documento.Estado;
        }
    }
}
