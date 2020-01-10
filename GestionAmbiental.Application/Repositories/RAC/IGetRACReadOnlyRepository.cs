using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.RAC
{
    public interface IGetRACReadOnlyRepository
    {
        Task<List<Domain.RAC.RAC>> GetRACs(DateTime fecha, string codigo);
        Task<Domain.RAC.RAC> GetRAC(int Id_RAC);
        Task<List<Domain.RAC.RAC>> GetRACsVerificacion(DateTime fecha, string codigo);
    }
}
