using GestionAmbiental.Domain.ResiduosSolidos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlIngresoSalidaRS
{
    public interface IGetIngresoSalidaRSReadOnlyRepository
    {
        Task<List<Ingreso_Salida_RS>> GetIngreso_Salida_RSs(int acr_cmrs, int ingreso_salida, int tipo, DateTime? fecha, int empresa, int gerencia);
        Task<Ingreso_Salida_RS> GetIngreso_Salida_RS(int id_Ingreso_Salida_RS);
        Task<List<Ingreso_Salida_RS>> GetIngreso_Salida_RSDescarga(DateTime fechaInicio, DateTime fechaFin);
        Task<string> GetLastItem();
    }
}
