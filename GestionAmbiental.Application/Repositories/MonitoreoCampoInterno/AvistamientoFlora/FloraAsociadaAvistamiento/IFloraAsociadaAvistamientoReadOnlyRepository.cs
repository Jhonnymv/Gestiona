using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento
{
    public interface IFloraAsociadaAvistamientoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento>> ListUsoAvistamiento();

        //Registrar
        Task<int> RegistrarFloraAsociadaAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento floraAsociada);

        //Editar
        Task EditarFloraAsociadaAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento floraAsociada);

        //buscar relación
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.FloraAsociadaAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento);
    }
}
