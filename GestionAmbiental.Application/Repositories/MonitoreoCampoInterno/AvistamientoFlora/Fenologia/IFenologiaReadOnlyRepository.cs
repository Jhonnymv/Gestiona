using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Fenologia
{
    public interface IFenologiaReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Fenologia>> ListFenologiaMCIF();

        //Registrar
        Task<int> RegistrarFenologiaMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Fenologia fenologia);

        //Editar
        Task EditarFenologiaMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.Fenologia fenologia);
    }
}
