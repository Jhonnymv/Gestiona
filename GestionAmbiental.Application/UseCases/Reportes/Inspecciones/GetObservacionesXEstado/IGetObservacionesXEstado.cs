using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetObservacionesXEstado
{
    public interface IGetObservacionesXEstado
    {
        Task<List<Dimension>> Execute();
    }
}
