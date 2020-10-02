using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.EditarUsuario
{
    public interface IEditarUsuario
    {
        Task Execute(int idUsuario, string nombres, string apellidos, int idRol, string email, string usuarioRed, string estado);
    }
}
