using GestionAmbiental.Domain.AvistamientoFauna.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.AvistamientoFauna
{
    public interface IGetItemsAvistamientoFaunaReadOnlyRepository
    {
        Task<List<TipoFauna>> GetTiposFauna();
        Task<List<NombreComunFauna>> GetNombresComunesFauna();
        Task<List<TipoAvistamiento>> GetTiposAvistamiento();
        Task<List<EstadoFauna>> GetEstadosFauna();
        Task<List<AreaTrabajo>> GetAreasTrabajo();
        Task<List<Empresa>> GetEmpresas();
        Task<List<Usuario>> GetUsuarios();
        Task<List<LugarReferencia>> GetLugaresReferencia();
        Task<List<ContactoEmergencia>> GetContactosEmergencia();
    }
}
