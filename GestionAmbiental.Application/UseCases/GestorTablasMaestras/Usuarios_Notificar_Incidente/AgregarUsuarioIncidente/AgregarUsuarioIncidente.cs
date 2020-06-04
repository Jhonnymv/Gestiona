using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.AgregarUsuarioIncidente
{
    public class AgregarUsuarioIncidente : IAgregarUsuarioIncidente
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarUsuarioIncidente(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Usuario_AD)
        {
            return await _gestorTablasMaestrasReadOnlyRepository.AddUsuario_Notificar_Incidente(Id_Usuario_AD);
        }
    }
}
