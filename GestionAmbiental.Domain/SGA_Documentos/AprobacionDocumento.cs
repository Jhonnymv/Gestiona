using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class AprobacionDocumento
    {
        public int Id_SGA_Revision_Documento { get; set; }
        public int Id_SGA_Documento { get; set; }
        public int Id_Usuario { get; set; }
        public int Orden { get; set; }
        public DateTime Fecha_Limite { get; set; }
        public DateTime? Fecha_Aprobacion { get; set; }
        public string Estado { get; set; }
    }
}
