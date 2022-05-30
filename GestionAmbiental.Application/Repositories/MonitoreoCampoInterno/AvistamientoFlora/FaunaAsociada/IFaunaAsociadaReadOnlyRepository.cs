using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada
{
    public interface IFaunaAsociadaReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada>> ListFaunaAsociada();

        //Registrar
        Task<int> RegistrarFaunaAsociada(Domain.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada faunaAsociada);

        //Editar
        Task EditarFaunaAsociada(Domain.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociada faunaAsociada);
    }
}
