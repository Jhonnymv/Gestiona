using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaUsuarioMCB
{
    public interface IGerenciaUsuarioReadOnlyRepository
    {
        //Listar
        Task<List<Domain.CompromisosMCB.GerenciaUsuarioCMCB>> ListarGerenciaUsuario();
        //Buscar por ID
        Task<Domain.CompromisosMCB.GerenciaUsuarioCMCB> BuscarPorID(int Id);
        //Registrar
        Task<int> RegistraGerenciaUsuario(Domain.CompromisosMCB.GerenciaUsuarioCMCB input);
        //Editar
        Task EditarGerenciaUsuario(Domain.CompromisosMCB.GerenciaUsuarioCMCB input);
        //Eliminar
        Task EliminarGerenciaUsuario(int IdEliminado);
    }
}
