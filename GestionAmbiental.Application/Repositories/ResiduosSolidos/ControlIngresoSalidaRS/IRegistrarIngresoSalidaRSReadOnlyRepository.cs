using GestionAmbiental.Domain.ResiduosSolidos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlIngresoSalidaRS
{
    public interface IRegistrarIngresoSalidaRSReadOnlyRepository
    {
        Task<int> RegistrarIngresoSalida(Ingreso_Salida_RS ingreso_Salida_RS);
    }
}
