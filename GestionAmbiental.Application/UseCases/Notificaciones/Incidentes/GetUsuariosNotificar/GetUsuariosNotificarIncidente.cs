using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.Repositories.Incidentes.Items;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.GetUsuarios_RS;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.GetUsuariosNotificar
{
    public class GetUsuariosNotificarIncidente : IGetUsuariosNotificarIncidente
    {
        private readonly IGetUsuariosNotificarReadOnlyRepository _getUsuariosNotificarReadOnlyRepository;

        public GetUsuariosNotificarIncidente(IGetUsuariosNotificarReadOnlyRepository getUsuariosNotificarReadOnlyRepository)
        {
            _getUsuariosNotificarReadOnlyRepository = getUsuariosNotificarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getUsuariosNotificarReadOnlyRepository));
        }

        public async Task<List<UsuarioOutput>> Execute()
        {
            var usuario = await _getUsuariosNotificarReadOnlyRepository.GetUsuariosNotificarIncidente();

            var usuariosOutput = new List<UsuarioOutput>();
            foreach (var item in usuario)
            {
                var temp = new UsuarioOutput();
                temp.IdUsuario = item.Id_Usuario_Notificar_Incidente;
                temp.Nombres = item.Nombres;
                temp.Apellidos = item.Apellidos;
                temp.UsuarioRed = item.Usuario_Red;
                temp.Estado = item.Estado;
                temp.Email = item.Email;
                temp.Rol = ((InspeccionRol)item.Id_Rol).ToString();
                usuariosOutput.Add(temp);
            }
            return usuariosOutput;
        }
    }
}
