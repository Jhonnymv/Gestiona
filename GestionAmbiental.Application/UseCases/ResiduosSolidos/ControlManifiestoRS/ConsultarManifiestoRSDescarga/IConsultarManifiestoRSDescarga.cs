using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRSDescarga
{
    public interface IConsultarManifiestoRSDescarga
    {
        Task<List<ManifiestoRSOutput>> Execute(string FechaInicio, string FechaFin);
    }
}
