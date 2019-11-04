using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarDetalles
{
    public class ConsultarDetalleImplementacion : IConsultarDetalleImplementacion
    {
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnly;

        public ConsultarDetalleImplementacion(IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnly)
        {
            _detalleInspeccionReadOnly = detalleInspeccionReadOnly ?? throw new ArgumentNullException(nameof(detalleInspeccionReadOnly));
        }

        public async Task<InspeccionDetalle> Execute(int IdObservacion)
        {
            var output = await _detalleInspeccionReadOnly.GetInspeccionDetalle(IdObservacion);
            return output;
        }
    }
}
