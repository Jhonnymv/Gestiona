using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.ValueObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
                var listaEmpresas = await _gestorUsuarioReadOnlyRepository.GetEmpresas();
                var listaAreas = await _gestorUsuarioReadOnlyRepository.GetAreasTrabajo();

                var data = JArray.Parse(usuarios);

                foreach (var usuario in data)
                {
                    var jObject = usuario;

                    var empresaUsuario = jObject.Value<string>("Empresa");
                    var areaUsuario = jObject.Value<string>("Area");
                    

                    var nuevoUsuario = new Usuario
                    {
                        Nombres = jObject.Value<string>("Nombres"),
                        Apellidos = jObject.Value<string>("Apellidos"),
                        UsuarioRed = jObject.Value<string>("UsuarioRed"),
                        Email = jObject.Value<string>("Email"),
                        DNI = jObject.Value<string>("DNI"),
                        Celular = jObject.Value<string>("Celular"),
                        Puesto = jObject.Value<string>("Puesto")
                    };

                    if (empresaUsuario != null)
                    {
                        var empresa = listaEmpresas.FirstOrDefault(x => x.RazonSocial.Equals(empresaUsuario));
                        if (empresa!=null)
                            nuevoUsuario.Empresa = new Empresa { IdEmpresa = empresa.IdEmpresa };
                        else
                        {
                            var IdEmpresa = await _gestorUsuarioReadOnlyRepository.AgregarEmpresa(new Empresa{
                                RazonSocial = empresaUsuario
                            });
                            nuevoUsuario.Empresa = new Empresa { IdEmpresa = IdEmpresa };
                        }
                    }
                    if (areaUsuario != null)
                    {
                        var area = listaAreas.FirstOrDefault(x => x.Descripcion.Equals(areaUsuario));
                        if (area != null)
                            nuevoUsuario.AreaTrabajo = new AreaTrabajo { IdAreaTrabajo = area.IdAreaTrabajo };
                        else
                        {
                            var IdArea = await _gestorUsuarioReadOnlyRepository.AgregarAreaTrabajo(new AreaTrabajo
                            {
                                Descripcion = areaUsuario
                            });
                            nuevoUsuario.AreaTrabajo = new AreaTrabajo { IdAreaTrabajo = IdArea };
                        }

                    }
                    lista_usuarios.Add(nuevoUsuario);
                }


                await _gestorUsuarioReadOnlyRepository.ActualizarUsuarios(lista_usuarios);
            }
        }
    }
}
