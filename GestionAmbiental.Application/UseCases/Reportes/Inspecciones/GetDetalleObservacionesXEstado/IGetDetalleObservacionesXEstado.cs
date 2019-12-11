using GestionAmbiental.Domain.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetDetalleObservacionesXEstado
{
    public interface IGetDetalleObservacionesXEstado
    {
        Task<List<DetalleObservacionXEstado>> Execute(string Estado);
    }
}
