using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.GetUsuario_RS
{
    public interface IGetUsuario_RS
    {
        Task<UsuarioOutput> Execute(int idUsuario);
    }
}
