using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ConsumoAgua
{
    public interface IGetConsumoAguaReadOnlyRepository
    {
        Task<Domain.ConsumoAgua.ConsumoAgua> GetConsumoAgua(int Id);
        Task<List<Domain.ConsumoAgua.ConsumoAgua>> GetConsumosAgua(DateTime? fecha);
        Task<List<Domain.ConsumoAgua.ConsumoAgua>> GetConsumosAguaDescarga(DateTime fechaInicio, DateTime fechaFina, int idResolucion);
        Task<List<Domain.ConsumoAgua.ConsumoAgua>> GetConsumosAguaDescarga(int idResolucion);
    }
}
