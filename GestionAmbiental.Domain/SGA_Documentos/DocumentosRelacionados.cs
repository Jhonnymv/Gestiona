using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.SGA_Documentos
{
    public class DocumentosRelacionados
    {
        public int Id_SGA_Documentos_Relacionados { get; set; }
        public int Id_Documento_Principal { get; set; }
        public int Id_Documento_Secundario { get; set; }
        public DateTime Fecha_Relacion { get; set; }
        public string Estado { get; set; }
    }
}
