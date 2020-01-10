using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Verificacion.AprobarObservacion
{
    public class AprobarObservacion : IAprobarObservacion
    {
        private readonly IVerificarObservacionRACReadOnlyRepository _verificarObservacionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public AprobarObservacion(IVerificarObservacionRACReadOnlyRepository verificarObservacionReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _verificarObservacionReadOnlyRepository = verificarObservacionReadOnlyRepository ?? throw new ArgumentNullException(nameof(verificarObservacionReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdRAC, string Comentario_Verificacion, string usuario_red)
        {
            var reporte = new Domain.RAC.RAC
            {
                Id_RAC = IdRAC,
                Comentario_Verificacion = Comentario_Verificacion
            };

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            await _verificarObservacionReadOnlyRepository.VerificarObservacionRAC(reporte, Id_Usuario, EstadosRAC.Aprobado);
        }
    }
}
