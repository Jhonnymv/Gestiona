using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Uso
{
    public interface IUsoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Uso>> ListUsos();

        //Registrar
        Task<int> RegistrarUsosMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Uso uso);

        //Editar
        Task EditarUsosMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Uso uso);
    }
}
