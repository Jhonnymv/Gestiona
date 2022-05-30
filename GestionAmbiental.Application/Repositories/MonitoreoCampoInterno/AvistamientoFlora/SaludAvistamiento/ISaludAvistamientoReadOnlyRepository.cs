using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento
{
    public interface ISaludAvistamientoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento>> ListSaludAvistamiento();

        //Registrar
        Task<int> RegistrarSaludAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento saludAvistamiento);

        //Editar
        Task EditarSaludAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento saludAvistamiento);

        //buscar relación
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.SaludAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento);
    }
}
