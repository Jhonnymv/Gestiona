using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FaunaAsociadaAvistamiento
{
    public interface IFaunaAsociadaReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.faunaAsociadaAvistamiento>> ListFaunaAsociadaAvistamiento();

        //Registrar
        Task<int> RegistrarFaunaAsociadaAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.faunaAsociadaAvistamiento faunaAsociadaAvistamiento);

        //Editar
        Task EditarFaunaAsociadaAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.faunaAsociadaAvistamiento faunaAsociadaAvistamiento);

        //buscar relación
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.faunaAsociadaAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento);
    }
}
