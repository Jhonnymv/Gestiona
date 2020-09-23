using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ConsumoAgua
{
    public interface IEditarConsumoAguaReadOnlyRepository
    {
        Task EditarConsumoAgua(Domain.ConsumoAgua.ConsumoAgua consumoAgua);
    }
}
