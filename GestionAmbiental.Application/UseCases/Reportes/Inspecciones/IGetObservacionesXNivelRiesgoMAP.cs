using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones
{
    public interface IGetObservacionesXNivelRiesgoMAP
    {
        Task<List<Dimension>> Execute();
    }
}
