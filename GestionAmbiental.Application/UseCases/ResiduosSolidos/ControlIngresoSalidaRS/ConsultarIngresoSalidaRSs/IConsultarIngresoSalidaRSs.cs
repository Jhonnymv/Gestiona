using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRSs
{
    public interface IConsultarIngresoSalidaRSs
    {
        Task<IngresoSalidaRSsOutput> Execute(int acr_cmrs, int ingreso_salida, int tipo, string fecha, int empresa, int asesorMA, int pagina, int rpp);
    }
}
