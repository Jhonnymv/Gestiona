using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.CalidadAgua
{
    public interface ICalidadAguaReadOnlyRepository
    {
        //Listar
        Task<List<Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua>> ListCalidadAgua();

        //Registrar
        Task<int> RegistrarCalidaAgua(Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua calidadAgua);

        //Editar
        Task EditarCalidadAgua(Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua calidadAgua);

        ////Buscar por ID
        Task<Domain.MonitoreoCampoInterno.CalidadAgua.CalidadAgua> BuscarPorID(int Id);
    }
}
