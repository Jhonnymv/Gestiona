using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Test
{
    public interface ITestListaReadOnlyRepository
    {
        //Listar
        Task<List<Domain.Test.TestLista>> ListarTestLista();     
        //Buscar por ID
        Task<Domain.Test.TestLista> BuscarPorID(int Id);
        //Registrar
        Task<int> RegistrarTestLista(Domain.Test.TestLista input);
        //Editar
        Task EditarTestLista(Domain.Test.TestLista input);
        //Eliminar
        Task EliminarTestLista(int IdEliminado);
    }
}
