using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos
{
    public class RevisionAprobacionInput
    {
        public int IdUsuario { get; set; }
        public int Orden { get; set; }
        public string FechaLimite { get; set; }
    }
}
