using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Inspecciones
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<ItemsSelectResumen> GetItemsResumen();
        Task<List<Empresa>> GetEmpresaContratistas();
        Task<List<Gerencia>> GetGerencias();
        Task<List<TipoCondicion>> GetTiposCondicion();
        Task<List<Usuario>> GetUsuarios();
    }
}
