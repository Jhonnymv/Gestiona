using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EliminarAccion
{
    public interface IEliminarAccionIncidente
    {
        Task Execute(int IdAccion);
    }
}
