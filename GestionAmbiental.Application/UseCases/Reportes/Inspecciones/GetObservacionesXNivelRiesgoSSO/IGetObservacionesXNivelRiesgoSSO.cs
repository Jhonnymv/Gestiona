using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXNivelRiesgoSSO
{
    public interface IGetObservacionesXNivelRiesgoSSO
    {
        Task<List<Dimension>> Execute();
    }
}
