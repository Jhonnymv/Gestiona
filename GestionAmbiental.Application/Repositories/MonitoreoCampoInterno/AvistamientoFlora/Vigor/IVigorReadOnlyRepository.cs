using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Vigor
{
    public interface IVigorReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Vigor>> ListVigor();

        //Registrar
        Task<int> RegistrarVigorMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Vigor vigor);

        //Editar
        Task EditarVigorMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Vigor vigor);
    }
}
