using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos
{
    public class DocumentoOutput
    {
        public int IdSGADocumento { get; set; }
        public int IdSGACategoriaTipo { get; set; }
        public int IdSGATipoDocumento { get; set; }
        public string SGATipoDocumento { get; set; }
        public int IdSGACategoriaDocumento { get; set; }
        public string SGACategoriaDocumento { get; set; }
        public string DocCodigo { get; set; }
        public string DocNombre { get; set; }
        public double DocVersion { get; set; }
        public int? IdDocPredecesor { get; set; }
        public string DocPredecesorNombre { get; set; }
        public string DocPredecesorCodigo { get; set; }
        public int? IdDocPrincipal { get; set; }
        public string DocPrincipalNombre { get; set; }
        public string DocPrincipalCodigo { get; set; }
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
        public List<RevisionDocumento> Revisores { get; set; } 
        public List<RevisionDocumento> Aprobadores { get; set; } 
        public string Estado { get; set; }

        public DocumentoOutput(Documento documento)
        {
            IdSGADocumento = documento.IdSGADocumento;
            IdSGACategoriaTipo = documento.SGACategoriaTipo.IdSGACategoriaTipo;
            IdSGATipoDocumento = documento.SGACategoriaTipo.TipoDocumento.IdSGATipoDocumento;
            SGATipoDocumento = documento.SGACategoriaTipo.TipoDocumento.Descripcion;
            IdSGACategoriaDocumento = documento.SGACategoriaTipo.CategoriaDocumento.Id_SGA_Categoria_Documento;
            SGACategoriaDocumento = documento.SGACategoriaTipo.CategoriaDocumento.Descripcion;
            DocCodigo = documento.DocCodigo;
            DocNombre = documento.DocNombre;
            DocVersion = documento.DocVersion;
            if (documento.DocPredecesor != null)
            {
                IdDocPredecesor = documento.DocPredecesor.IdSGADocumento;
                DocPredecesorNombre = documento.DocPredecesor.DocNombre;
                DocPredecesorCodigo = documento.DocPredecesor.DocCodigo;
            }
            if (documento.DocPrincipal != null)
            {
                IdDocPrincipal = documento.DocPrincipal.IdSGADocumento;
                DocPrincipalNombre = documento.DocPrincipal.DocNombre;
                DocPrincipalCodigo = documento.DocPrincipal.DocCodigo;
            }
            FechaAprobacion = documento.FechaAprobacion;
            IdCondicion = documento.Condicion.IdSGACondicion;
            Condicion = documento.Condicion.Descripcion;
            Comentario = documento.Comentario;
            FechaTentativa = documento.FechaTentativa;
            FechaActualizacion = documento.FechaActualizacion;
            Estado = documento.Estado;
            Revisores = documento.Revisiones;
            Aprobadores = documento.Aprobaciones;
            DocRuta = documento.DocRuta;
            IdUsuario = documento.Usuario.IdUsuario;
            Usuario = documento.Usuario.Nombres + " " + documento.Usuario.Apellidos;
        }
    }
}
