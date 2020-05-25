using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.GetUsuariosNotificar
{
    public interface IGetUsuariosNotificarIncidente
    {
        Task<List<UsuarioOutput>> Execute();
    }
}
