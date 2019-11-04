using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarDetalles
{
    public interface IConsultarDetalleImplementacion
    {
        Task<InspeccionDetalle> Execute(int IdObservacion);
    }
}
