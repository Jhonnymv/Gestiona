using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.GestorUsuarios
{
    public interface IGestorUsuarioReadOnlyRepository
    {
        Task<InspeccionRol?> GetRolUsuario(string usuario_red);
        Task<Usuario> GetUsuario(string usuario_red);
        Task<int> GetIdUsuario(string usuario_red);
        Task ActualizarUsuarios(List<Usuario> usuarios);
    }
}
