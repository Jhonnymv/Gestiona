using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.VerificarObservacion
{
    public class VerificarObservacion : IVerificarObservacion
    {
        private readonly IVerificarObservacionReadOnlyRepository _verificarObservacionReadOnlyRepository;

        public VerificarObservacion(IVerificarObservacionReadOnlyRepository verificarObservacionReadOnlyRepository)
        {
            _verificarObservacionReadOnlyRepository = verificarObservacionReadOnlyRepository ?? throw new ArgumentNullException(nameof(verificarObservacionReadOnlyRepository));
        }

        public async Task Execute(int IdObservacion, string Comentario,string Estado)
        {
            var observacion = new NuevaInspeccionDetalle();
            observacion.Estado = Estado;
            observacion.Id_Inspeccion_Detalle = IdObservacion;
            observacion.Comentario_Inspector = Comentario;
            await _verificarObservacionReadOnlyRepository.VerificarObservacion(observacion);
        }
    }
}
