using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos
{
    public class RevisionAprobacionInput
    {
        public int Usuario { get; set; }
        public string UsuarioNombre { get; set; }
        public int Orden { get; set; }
        public string Fecha { get; set; }
    }
}
