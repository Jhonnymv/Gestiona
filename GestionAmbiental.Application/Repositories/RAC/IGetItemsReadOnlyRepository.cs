using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.RAC
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<Empresa>> GetEmpresas();
        Task<List<Gerencia>> GetGerencias();
        Task<List<TipoCondicion>> GetTiposCondicion();
        Task<List<Usuario>> GetUsuarios();
    }
}

