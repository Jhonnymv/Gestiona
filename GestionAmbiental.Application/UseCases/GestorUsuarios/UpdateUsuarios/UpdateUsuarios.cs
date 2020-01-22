using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.ValueObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.UpdateUsuarios
{
    public class UpdateUsuarios : IUpdateUsuarios
    {
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public UpdateUsuarios(IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(string usuarios)
        {
            var lista_usuarios = new List<Usuario>();
            if (usuarios != null)
            {
                var data = JArray.Parse(usuarios);

                foreach (var usuario in data)
                {
                    var jObject = usuario;
                    lista_usuarios.Add(new Usuario
                    {
                        Nombres = jObject.Value<string>("FirstName"),
                        Apellidos = jObject.Value<string>("LastName"),
                        UsuarioRed = jObject.Value<string>("UserName"),
                        Email = jObject.Value<string>("Email"),
                    });
                }
                await _gestorUsuarioReadOnlyRepository.ActualizarUsuarios(lista_usuarios);
            }
        }
    }
}
