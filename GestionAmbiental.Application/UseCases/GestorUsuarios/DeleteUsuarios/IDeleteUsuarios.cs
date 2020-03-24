using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.DeleteUsuarios
{
    public interface IDeleteUsuario
    {
        Task Execute(int Id_Usuarios);
    }
}
