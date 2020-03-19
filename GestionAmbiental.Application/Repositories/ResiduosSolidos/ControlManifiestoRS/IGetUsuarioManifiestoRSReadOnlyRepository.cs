using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS
{
    public interface IGetUsuarioManifiestoRSReadOnlyRepository
    {
        Task<List<Usuario_Notificar_RS>> GetUsuariosNotificarRS();
    }
}
