using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento
{
    public interface IUsoAvistamientoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento>> ListUsoAvistamiento();

        //Registrar
        Task<int> RegistrarUsoAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento usoAvistamiento);

        //Editar
        Task EditarUsoAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento usoAvistamiento);

        //buscar relación
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.UsoAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento);
    }
}
