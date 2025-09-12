using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.ImplementarAccionMA
{
    public interface IImplementarAccionMA
    {
        Task<int> Execute(int IdAccion, List<string> evidencias, string comentario);
    }
}
