using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.VerificarObservacion
{
    public class VerificarObservacionMA : IVerificarObservacionMA
    {
        private readonly IVerificarObservacionReadOnlyRepository _verificarObservacionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        public VerificarObservacionMA(IVerificarObservacionReadOnlyRepository verificarObservacionReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _verificarObservacionReadOnlyRepository = verificarObservacionReadOnlyRepository ?? throw new ArgumentNullException(nameof(verificarObservacionReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdObservacion, string Comentario, string Estado,string usuario_red)
        {
            var observacion = new NuevaInspeccionDetalle();
            observacion.Estado = Estado;
            observacion.Id_Inspeccion_Detalle = IdObservacion;
            observacion.Comentario_MA = Comentario;

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            await _verificarObservacionReadOnlyRepository.VerificarObservacionMA(observacion,Id_Usuario);
        }
    }
}
