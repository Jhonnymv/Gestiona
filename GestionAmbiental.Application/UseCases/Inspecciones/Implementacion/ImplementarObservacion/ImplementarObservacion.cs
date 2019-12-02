using GestionAmbiental.Application.Repositories.GestorUsuarios;
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
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        public ImplementarObservacion(
            ILevantarObservacionesReadOnlyRepository levantarObservacionesReadOnlyRepository,
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _levantarObservacionesReadOnlyRepository = levantarObservacionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(levantarObservacionesReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdObservacion, List<string> Evidencia_Inspeccion, string Comentario,string usuario_red)
        {
            var evidencia = JsonConvert.SerializeObject(Evidencia_Inspeccion);
            var observacion = new NuevaInspeccionDetalle();
            observacion.Id_Inspeccion_Detalle = IdObservacion;
            observacion.Evidencia_Inspeccion = evidencia;
            observacion.Comentario_Inspeccion = Comentario;

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
            
            await _levantarObservacionesReadOnlyRepository.LevantarObservacion(observacion,Id_Usuario);


        }
    }
}
