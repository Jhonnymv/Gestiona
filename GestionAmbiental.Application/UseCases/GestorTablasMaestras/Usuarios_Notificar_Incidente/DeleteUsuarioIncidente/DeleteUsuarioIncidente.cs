using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.DeleteUsuarioIncidente
{
    public class DeleteUsuarioIncidente : IDeleteUsuarioIncidente
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public DeleteUsuarioIncidente(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int Id_Usuario)
        {
            await _gestorTablasMaestrasReadOnlyRepository.QuitarUsuario_Notificar_Incidente(Id_Usuario);
        }
    }
}
