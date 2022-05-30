using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento
{
    public interface IFenologiaAvistamientoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento>> ListFenologiaAsociadaAvistamiento();

        //Registrar
        Task<int> RegistrarFenologiaAsociadaAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento fenologiaAvistamiento);

        //Editar
        Task EditarFenologiaAsociadaAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento fenologiaAvistamiento);

        //buscar relación
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.FenologiaAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento);
    }
}
