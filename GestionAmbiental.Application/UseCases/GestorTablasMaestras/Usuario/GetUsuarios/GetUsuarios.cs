using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios
{
    public class GetUsuarios : IGetUsuarios
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetUsuarios(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<UsuariosOutput> Execute(string filtro, int pagina, int rpp)
        {
            var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuarios(filtro);

            var usuariosOutput = new List<UsuarioOutput>();
            foreach (var item in usuario)
            {
                var temp = new UsuarioOutput();
                temp.IdUsuario = item.IdUsuario;
                temp.Nombres = item.Nombres;
                temp.Apellidos = item.Apellidos;
                temp.UsuarioRed = item.UsuarioRed;
                temp.Estado = item.Estado;
                temp.Email = item.Email;
                temp.Rol = ((InspeccionRol)item.IdRol).ToString();
                usuariosOutput.Add(temp);
            }
            var output = new UsuariosOutput
            {
                nUsuarios = usuariosOutput.Count,
                Usuarios = usuariosOutput.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }

        public async Task<UsuariosOutput> Execute()
        {
            var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuarios("");
            var usuariosOutput = new List<UsuarioOutput>();
            foreach (var item in usuario)
            {
                if(item.Estado.Equals("ACT"))
                {
                    var temp = new UsuarioOutput();
                    temp.IdUsuario = item.IdUsuario;
                    temp.Nombres = item.Nombres;
                    temp.Apellidos = item.Apellidos;
                    temp.UsuarioRed = item.UsuarioRed;
                    temp.Estado = item.Estado;
                    temp.Email = item.Email;
                    temp.Rol = ((InspeccionRol)item.IdRol).ToString();
                    usuariosOutput.Add(temp);
                }
            }
            var output = new UsuariosOutput
            {
                nUsuarios = usuariosOutput.Count,
                Usuarios = usuariosOutput
            };
            return output;
        }
    }
}
