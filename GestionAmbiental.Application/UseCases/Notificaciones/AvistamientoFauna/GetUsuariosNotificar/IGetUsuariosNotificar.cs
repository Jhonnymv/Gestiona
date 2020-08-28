using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.AvistamientoFauna.GetUsuariosNotificar
{
    public interface IGetUsuariosNotificar
    {
        Task<List<UsuarioOutput>> Execute();
    }
}
