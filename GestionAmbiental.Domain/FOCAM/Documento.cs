using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.FOCAM
{
    public class Documento
    {
        public int Id { get; set; }

        public Tipo Tipo { get; set; }
        public string Codigo { get; set; }
        public string Titulo { get; set; }
        public string Version { get; set; }
        public DateTime FechaAprovacion { get; set; }
        public string Estado { get; set; }
        public string Comentarios { get; set; }
        public DateTime FechaTentaivaVersion { get; set; }
        public string UrlDocumento { get; set; }
        public Documento()
        {

        }
    }
}
