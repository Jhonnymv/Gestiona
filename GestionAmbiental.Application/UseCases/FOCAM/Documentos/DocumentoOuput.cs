using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.FOCAM.Documentos
{
    public class DocumentoOuput
    {
        public int Id { get; set; }
        public int? IdTipo { get; set; }
        public string NombreTipo { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Version { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public string Estado { get; set; }
        public string Comentarios { get; set; }
        public DateTime FechaTentaivaVersion { get; set; }
        public string UrlDocumento { get; set; }

        public DocumentoOuput()
        {

        }

        public DocumentoOuput(Domain.FOCAM.Documento docuemnto)
        {
            Id = docuemnto.Id;
            IdTipo = docuemnto.Tipo.Id;
            NombreTipo = docuemnto.Tipo.Nombre;
            Codigo = docuemnto.Codigo;
            Titulo = docuemnto.Titulo;
            Version = docuemnto.Version;
            FechaAprobacion = docuemnto.FechaAprovacion;
            Estado = docuemnto.Estado;
            Comentarios = docuemnto.Comentarios;
            FechaTentaivaVersion=docuemnto.FechaTentaivaVersion;
            UrlDocumento = docuemnto.UrlDocumento;
        }
    }
   
   
}
