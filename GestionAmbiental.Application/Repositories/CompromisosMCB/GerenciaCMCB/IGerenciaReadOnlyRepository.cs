using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaCMCB
{
    public interface IGerenciaReadOnlyRepository
    {
        Task<List<Domain.CompromisosMCB.GerenciaCMCB>> ListarGerencias();
        //Registrar
        Task<int> RegistrarGerencia(Domain.CompromisosMCB.GerenciaCMCB gerenciaCMCB);

        //Editar
        Task EditarGerencia(Domain.CompromisosMCB.GerenciaCMCB gerenciaCMCB);
    }
}
