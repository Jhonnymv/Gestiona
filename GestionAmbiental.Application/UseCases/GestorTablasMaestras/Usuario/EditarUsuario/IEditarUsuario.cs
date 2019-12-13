using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.EditarUsuario
{
    public interface IEditarUsuario
    {
        Task Execute(int idUsuario, string nombres, string apellidos, int idRol, int? idSuperior, string email, string usuarioRed, string estado);
    }
}
