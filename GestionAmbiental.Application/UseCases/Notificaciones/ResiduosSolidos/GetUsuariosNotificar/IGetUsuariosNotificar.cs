using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ResiduosSolidos.GetUsuariosNotificar
{
    public interface IGetUsuariosNotificar
    {
        Task<List<UsuarioNotificarOutput>> Execute();
    }
}
