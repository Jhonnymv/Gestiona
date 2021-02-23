using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.SGA_Documentos
{
    public class RevisoresAprobadoresInput
    {
        public int Orden { get; set; }
        public int Usuario { get; set; }
        public string UsuarioNombre { get; set; }
        public string Fecha { get; set; }
    }
}
