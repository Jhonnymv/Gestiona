using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarDetalles
{
    public class ConsultarDetallesVerificacion:IConsultarDetallesVerificacion
    {
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnly;

        public ConsultarDetallesVerificacion(IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnly)
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
