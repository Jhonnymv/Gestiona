using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.Zona
{
    public interface IZonaReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona>> ListZonas();

        //Registrar
        Task<int> RegistrarZona(Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona zona);

        //Editar
        Task EditarZona(Domain.MonitoreoCampoInterno.AvistamientoFlora.Zona zona);
    }
}
