using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento
{
    public interface INombreCientificoAvistamientoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento>> ListNombreCientificoAvistamiento();

        //Registrar
        Task<int> RegistrarNombreCientificoaAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento nombreCientificoAvistamiento);

        //Editar
        Task EditarNombreCientificoAvistamiento(Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento nombreCientificoAvistamiento);

        //buscar relación
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientificoAvistamiento>> BuscarPorAvistamientoId(int IdAvistamiento);
    }
}
