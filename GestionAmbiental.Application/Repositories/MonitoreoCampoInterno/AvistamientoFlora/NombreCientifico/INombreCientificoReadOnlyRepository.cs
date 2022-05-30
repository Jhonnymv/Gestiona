using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico
{
    public interface INombreCientificoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico>> ListNombreCientifico();

        //Registrar
        Task<int> RegistrarNombreCientificoMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico nombreCientifico);

        //Editar
        Task EditarNombreCientificoMCIF(Domain.MonitoreoCampoInterno.AvistamientoFlora.NombreCientifico nombreCientifico);
    }
}
