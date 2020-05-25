using GestionAmbiental.Domain.Incidentes.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.Items
{
    public interface IGetUsuariosNotificarReadOnlyRepository
    {
        Task<List<Usuario_Notificar_Incidente>> GetUsuariosNotificarIncidente();
    }
}
