using GestionAmbiental.Domain.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ConsumoAgua
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<ResolucionConsumoAgua>> GetReslucionesConsumoAgua();
    }
}
