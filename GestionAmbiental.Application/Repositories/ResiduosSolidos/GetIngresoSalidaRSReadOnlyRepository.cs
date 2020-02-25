using GestionAmbiental.Domain.ResiduosSolidos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos
{
    public interface GetIngresoSalidaRSReadOnlyRepository
    {
        Task<List<Ingreso_Salida_RS>> GetIngreso_Salida_RSs(int acr_cmrs, int ingreso_salida, int tipo, DateTime? fecha, int empresa, int asesorMA);
        Task<int> GetLastItem();
    }
}
