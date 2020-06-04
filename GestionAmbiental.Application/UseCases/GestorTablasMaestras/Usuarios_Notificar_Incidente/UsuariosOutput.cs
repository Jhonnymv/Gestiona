using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario_Notificar_Incidente
{
    public class UsuariosOutput
    {
        public List<UsuarioOutput> Usuarios { get; set; }
        public int nUsuarios { get; set; }
    }
}
