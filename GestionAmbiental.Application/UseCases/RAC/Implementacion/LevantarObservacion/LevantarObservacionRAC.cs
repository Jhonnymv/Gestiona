using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Implementacion.LevantarObservacion
{
    public class LevantarObservacionRAC : ILevantarObservacionRAC
    {
        private readonly ILevantarObservacionRACReadOnlyRepository _levantarObservacionRACReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public LevantarObservacionRAC(ILevantarObservacionRACReadOnlyRepository levantarObservacionRACReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _levantarObservacionRACReadOnlyRepository = levantarObservacionRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(levantarObservacionRACReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdRAC, string Evidencia_Cierre,string Comentario_Cierre, string usuario_red)
        {
            var reporte = new Domain.RAC.RAC
            {
                Id_RAC = IdRAC,
                Evidencia_Cierre = Evidencia_Cierre,
                Comentario_Cierre = Comentario_Cierre
            };

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            await _levantarObservacionRACReadOnlyRepository.LevantarObservacionRAC(reporte, Id_Usuario);
        }
    }
}
