using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora 
{ 
    public interface IAvistamientoFloraReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora>> ListAvistamientoFlora();

        //Registrar
        Task<int> RegistrarAvistamientoFlora(Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora avistamientoFlora);

        //Editar
        Task EditarAvistamientoFlora(Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora avistamientoFlora);

        ////Buscar por ID
        Task<Domain.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora> BuscarPorID(int Id);
    }
}
