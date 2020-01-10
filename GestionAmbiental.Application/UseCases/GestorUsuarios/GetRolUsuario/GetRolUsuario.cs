using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.GetRolUsuario
{
    public class GetRolUsuario : IGetRolUsuario
    {
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuariosReadOnlyRepository;

        public GetRolUsuario(IGestorUsuarioReadOnlyRepository gestorUsuariosReadOnlyRepository)
        {
            _gestorUsuariosReadOnlyRepository = gestorUsuariosReadOnlyRepository;
        }

        public async Task<InspeccionRol?> Execute(string usuario_red)
        {
            return await _gestorUsuariosReadOnlyRepository.GetRolUsuario(usuario_red);
        }
    }
}
