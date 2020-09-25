using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ConsumoAgua
{
    public interface IRegistrarConsumoAguaReadOnlyRepository
    {
        Task<int> RegistrarConsumoAgua(Domain.ConsumoAgua.ConsumoAgua consumo);
        Task<int> RegistrarConsumoAguaMasivo(List<Domain.ConsumoAgua.ConsumoAgua> consumos);
    }
}
