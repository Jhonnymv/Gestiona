using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.RAC
{
    public interface IRegistrarRACReadOnlyRepository
    {
        Task<int> RegistrarRAC(Domain.RAC.RAC nuevoRegistro);
        Task<string> GetUltimoCodigo();
    }
}
