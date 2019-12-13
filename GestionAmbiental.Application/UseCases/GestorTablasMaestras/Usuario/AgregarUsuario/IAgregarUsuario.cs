using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.AgregarUsuario
{
    public interface IAgregarUsuario
    {
        Task<int> Execute(string nombres, string apellidos, int idRol, int? idSuperior, string email, string usuarioRed);
    }
}
