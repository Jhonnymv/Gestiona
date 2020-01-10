using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario
{
    public interface IGetUsuario
    {
        Task<UsuarioOutput> Execute(string usuario_red);
    }
}
