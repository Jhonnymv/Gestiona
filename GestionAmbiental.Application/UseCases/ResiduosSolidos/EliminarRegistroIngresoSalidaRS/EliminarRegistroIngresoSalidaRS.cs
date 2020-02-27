using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.ResiduosSolidos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.EliminarRegistroIngresoSalidaRS
{
    public class EliminarRegistroIngresoSalidaRS : IEliminarRegistroIngresoSalidaRS
    {
        private readonly IEditarIngresoSalidaRSReadOnlyRepository _editarIngresoSalidaRSReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EliminarRegistroIngresoSalidaRS(IEditarIngresoSalidaRSReadOnlyRepository editarIngresoSalidaRSReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarIngresoSalidaRSReadOnlyRepository = editarIngresoSalidaRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarIngresoSalidaRSReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int Id_Ingreso_Salida_RS, string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            await _editarIngresoSalidaRSReadOnlyRepository.EliminarRegistroIngresoSalidaRS(Id_Ingreso_Salida_RS, usuario.IdUsuario);
            
        }
    }
}
