using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Avistamiento_Fauna.GetUsuario
{
    public interface IGetUsuario
    {
        Task<UsuarioOutput> Execute(int idUsuario);
    }
}
