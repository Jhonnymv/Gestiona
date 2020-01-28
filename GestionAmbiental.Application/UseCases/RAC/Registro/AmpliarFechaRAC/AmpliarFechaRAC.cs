using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.AmpliarFechaRAC
{
    public class AmpliarFechaRAC : IAmpliarFechaRAC
    {
        private readonly IEditarObservacionRACReadOnlyRepository _editarObservacionRACReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public AmpliarFechaRAC(IEditarObservacionRACReadOnlyRepository editarObservacionRACReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarObservacionRACReadOnlyRepository = editarObservacionRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarObservacionRACReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdRAC, string nuevaFecha,string usuario_red)
        {
            var fecha = Convert.ToDateTime(nuevaFecha);
            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);
            await _editarObservacionRACReadOnlyRepository.AmpliarFechaObservacionRAC(IdRAC, fecha,Id_Usuario);
        }
    }
}
