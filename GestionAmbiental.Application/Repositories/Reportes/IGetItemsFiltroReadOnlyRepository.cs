using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Reportes
{
    public interface IGetItemsFiltroReadOnlyRepository
    {
        Task<List<Empresa>> GetEmpresas();
        Task<List<Gerencia>> GetGerencias();
        Task<List<Usuario>> GetAsesoresMA();
        Task<List<Usuario>> GetAsesorSSO();
        Task<List<Usuario>> GetUsuarios();
    }
}
