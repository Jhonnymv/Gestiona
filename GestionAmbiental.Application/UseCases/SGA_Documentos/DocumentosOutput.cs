using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos
{
    public class DocumentosOutput
    {
        public List<DocumentoOutput> Documentos { get; set; }
        public int NRegistros { get; set; }
    }
}
