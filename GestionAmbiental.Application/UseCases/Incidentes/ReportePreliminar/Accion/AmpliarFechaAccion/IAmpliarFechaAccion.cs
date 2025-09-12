using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.AmpliarFechaAccion
{
    public interface IAmpliarFechaAccion
    {
        Task<int> Execute(int IdAccion,string Fecha);
    }
}
