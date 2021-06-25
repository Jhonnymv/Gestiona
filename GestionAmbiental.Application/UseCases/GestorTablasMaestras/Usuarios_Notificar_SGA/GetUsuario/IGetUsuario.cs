using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_SGA.GetUsuario
{
    public interface IGetUsuario
    {
        Task<UsuarioOutput> Execute(int idUsuario);
    }
}
