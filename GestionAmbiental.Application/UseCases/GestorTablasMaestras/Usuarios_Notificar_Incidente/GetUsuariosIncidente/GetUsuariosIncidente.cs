using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario_Notificar_Incidente;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Incidente.GetUsuariosIncidente
{
    public class GetUsuariosIncidente : IGetUsuariosIncidente
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetUsuariosIncidente(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<UsuariosOutput> Execute(string filtro, int pagina, int rpp)
        {
            var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario_Notificar_Incidente(filtro);

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
            var output = new UsuariosOutput
            {
                nUsuarios = usuariosOutput.Count,
                Usuarios = usuariosOutput.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
