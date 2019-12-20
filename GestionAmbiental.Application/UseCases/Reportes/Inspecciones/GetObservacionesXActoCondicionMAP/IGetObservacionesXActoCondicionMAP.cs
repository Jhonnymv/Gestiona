using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXActoCondicionMAP
{
    public interface IGetObservacionesXActoCondicionMAP
    {
        Task<List<Dimension>> Execute();
    }
}
