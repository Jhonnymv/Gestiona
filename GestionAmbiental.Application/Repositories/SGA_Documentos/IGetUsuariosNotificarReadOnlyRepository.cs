using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.SGA_Documentos
{
    public interface IGetUsuariosNotificarReadOnlyRepository
    {
        Task<List<Usuarios_Notificar_SGA>> GetUsuariosNotificarSGA();
    }
}
