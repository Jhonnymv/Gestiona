using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.CalidadAgua
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<Domain.ValueObjects.Usuario>> GetUsuarios();
    }
}
