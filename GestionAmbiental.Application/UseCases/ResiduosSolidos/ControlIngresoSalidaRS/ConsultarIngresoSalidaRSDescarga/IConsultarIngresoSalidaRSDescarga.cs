using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRSDescarga
{
    public interface IConsultarIngresoSalidaRSDescarga
    {
        Task<List<IngresoSalidaRSOutput>> Execute(string FechaInicio, string FechaFin);
    }
}
