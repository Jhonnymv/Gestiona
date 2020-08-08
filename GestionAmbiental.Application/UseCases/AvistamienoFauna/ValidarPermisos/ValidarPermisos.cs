using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientoFauna;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.ValidarPermisos
{
    public class ValidarPermisos : IValidarPermisos
    {
        private readonly IGetUsuario _getUsuario;

        public ValidarPermisos(IGetUsuario getUsuario, IGetAvistamientoFauna getAvistamientoFauna)
        {
            _getUsuario = getUsuario ?? throw new ArgumentNullException(nameof(getUsuario));
        }

        public async Task<bool> Editar(string UsuarioRed, int IdUsuarioAvistamiento)
        {
            var usuario = await _getUsuario.Execute(UsuarioRed);
            return usuario.IdUsuario.Equals(IdUsuarioAvistamiento);
        }
    }
}
