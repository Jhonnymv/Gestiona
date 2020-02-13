using GestionAmbiental.Application.Repositories.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarObservacion
{
    public class ConsultarObservacion: IConsultarObservacion
    {
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnlyRepository;

        public ConsultarObservacion(IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnlyRepository)
        {
            _detalleInspeccionReadOnlyRepository = detalleInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(detalleInspeccionReadOnlyRepository));
        }

        public async Task<InspeccionDetalleOutput> Execute(int IdObservacion)
        {
            var observacion = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalle(IdObservacion);
            var output = new InspeccionDetalleOutput(observacion);
            return output;
        }
    }
}
