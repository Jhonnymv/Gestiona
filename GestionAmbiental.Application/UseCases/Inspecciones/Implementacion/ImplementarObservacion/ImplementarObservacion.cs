using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ImplementarObservacion
{
    public class ImplementarObservacion : IImplementarObservacion
    {
        private readonly ILevantarObservacionesReadOnlyRepository _levantarObservacionesReadOnlyRepository;

        public ImplementarObservacion(ILevantarObservacionesReadOnlyRepository levantarObservacionesReadOnlyRepository)
        {
            _levantarObservacionesReadOnlyRepository = levantarObservacionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(levantarObservacionesReadOnlyRepository));
        }

        public async Task Execute(int IdObservacion, List<string> Evidencia_Inspeccion, string Comentario)
        {
            var evidencia = JsonConvert.SerializeObject(Evidencia_Inspeccion);
            var observacion = new NuevaInspeccionDetalle();
            observacion.Id_Inspeccion_Detalle = IdObservacion;
            observacion.Evidencia_Inspeccion = evidencia;
            observacion.Comentario_Inspeccion = Comentario;
            await _levantarObservacionesReadOnlyRepository.LevantarObservacion(observacion);
        }
    }
}
