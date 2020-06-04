using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario_Notificar_Incidente;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.GetUsuarioIncidente
{
    public interface IGetUsuarioIncidente
    {
        Task<UsuarioOutput> Execute(int idUsuario);
    }
}
