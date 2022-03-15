using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB
{
    public interface ICompromisoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.CompromisosMCB.CompromisoCMCB>> ListarCompromisos();
        ////Buscar por ID
        Task<Domain.CompromisosMCB.CompromisoCMCB> BuscarPorID(int Id);
        //Registrar
        Task<int> RegistrarCompromiso(Domain.CompromisosMCB.CompromisoCMCB input);
        ////Editar
        //Task EditarCompromiso(Domain.CompromisosMCB.CompromisoCMCB input);
        ////Eliminar
        //Task EliminarCompromiso(int IdEliminado);
    }
}
