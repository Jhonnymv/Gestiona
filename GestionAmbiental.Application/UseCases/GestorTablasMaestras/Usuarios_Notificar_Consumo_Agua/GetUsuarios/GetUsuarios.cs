using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_Consumo_Agua.GetUsuarios
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
            var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario_Notificar_Consumo_Agua(filtro);

            var usuariosOutput = new List<UsuarioOutput>();
            foreach (var item in usuario)
            {
                var temp = new UsuarioOutput(item);
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
