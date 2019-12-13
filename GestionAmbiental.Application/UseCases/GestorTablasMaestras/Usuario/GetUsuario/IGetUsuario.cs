using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario
{
    public interface IGetUsuario
    {
        Task<UsuarioOutput> Execute(int idUsuario);
    }
}
