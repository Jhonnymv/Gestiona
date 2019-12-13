using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.AgregarUsuario
{
    public class AgregarUsuario : IAgregarUsuario
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarUsuario(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string nombres, string apellidos, int idRol, int? idSuperior, string email, string usuarioRed)
        {
            var usuario = new Domain.ValueObjects.Usuario
            {
                Nombres = nombres,
                Apellidos = apellidos,
                IdRol = idRol,
                IdSuperior = idSuperior,
                Email = email,
                UsuarioRed = usuarioRed
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddUsuario(usuario);
        }
    }
}
