using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Salud
{
    public interface ISaludReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Salud>> ListSalud();

        //Registrar
        Task<int> RegistrarSaludMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Salud salud);

        //Editar
        Task EditarSaludMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Salud salud);
    }
}
