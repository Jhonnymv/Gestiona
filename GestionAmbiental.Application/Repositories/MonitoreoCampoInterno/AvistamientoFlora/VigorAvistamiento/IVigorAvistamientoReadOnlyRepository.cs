using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento
{
    public interface IVigorAvistamientoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento>> ListVigorAvistamiento();

        //Registrar
        Task<int> RegistrarVigorAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento vigorAvistamiento);

        //Editar
        Task EditarVigorAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento vigorAvistamiento);

        //buscar relación
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.VigorAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento);
    }
}
