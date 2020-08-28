using GestionAmbiental.Domain.AvistamientoFauna.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.AvistamientoFauna
{
    public interface IGetUsuariosNotificarReadOnlyRepository
    {
        Task<List<Usuario_Notificar_Avistamiento_Fauna>> GetUsuariosNotificarIncidente();
    }
}
