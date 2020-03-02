using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRS
{
    public interface IConsultarIngresoSalidaRS
    {
        Task<IngresoSalidaRSOutput> Execute(int Id_Ingreso_Salida_RS, string usuario_red);
    }
}
