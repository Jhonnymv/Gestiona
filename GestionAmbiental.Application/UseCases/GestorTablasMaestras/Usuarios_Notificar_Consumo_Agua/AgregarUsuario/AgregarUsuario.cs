using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.AgregarUsuario
{
    public class AgregarUsuario : IAgregarUsuario
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarUsuario(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Usuario_AD)
        {
            return await _gestorTablasMaestrasReadOnlyRepository.AddUsuario_Notificar_Consumo_Agua(Id_Usuario_AD);
        }
    }
}
