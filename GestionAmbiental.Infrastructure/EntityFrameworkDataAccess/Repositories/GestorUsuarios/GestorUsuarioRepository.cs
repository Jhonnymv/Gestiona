using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.GestorUsuarios
{
    public class GestorUsuarioRepository : IGestorUsuarioReadOnlyRepository
    {
        private Context _context;

        public GestorUsuarioRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<InspeccionRol?> GetRolUsuario(string usuario_red)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.usuario_red.Equals(usuario_red) && x.Estado.Equals("ACT"));

                if (usuario != null)
                {
                    var enumList = Enum.GetValues(typeof(InspeccionRol)).OfType<InspeccionRol>().ToList();

                    return enumList.FirstOrDefault(x => usuario.Id_Rol.Equals((int)x));

                    /*if (usuario.Id_Rol.Equals((int)InspeccionRol.Admin))
                        return InspeccionRol.Admin;
                    else if (usuario.Id_Rol.Equals((int)InspeccionRol.AsesorMAP))
                        return InspeccionRol.AsesorMAP;
                    else if (usuario.Id_Rol.Equals((int)InspeccionRol.AsesorSSO))
                        return InspeccionRol.AsesorSSO;
                    else
                        return InspeccionRol.Inspector;*/
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<Usuario> GetUsuario(string usuario_red)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => usuario_red.Equals(x.usuario_red) && x.Estado.Equals("ACT"));
            var areaTrabajo = await _context.Areas_Trabajo.FirstOrDefaultAsync(x => x.Id_Area_Trabajo.Equals(usuario.Id_Area_Trabajo));
            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(usuario.Id_Empresa));


            var output = new Usuario();
            output.Nombres = usuario.Nombres;
            output.Apellidos = usuario.Apellidos;
            output.Email = usuario.Email;
            output.UsuarioRed = usuario.usuario_red;
            output.IdUsuario = usuario.Id_Usuario;
            if (areaTrabajo != null)
                output.AreaTrabajo = new AreaTrabajo
                {
                    IdAreaTrabajo = areaTrabajo.Id_Area_Trabajo,
                    Descripcion = areaTrabajo.Descripcion,
                    Estado = areaTrabajo.Estado
                };
            if (empresa != null)
                output.Empresa = new Empresa
                {
                    IdEmpresa = empresa.Id_Empresa,
                    RazonSocial = empresa.Razon_Social,
                    Estado = empresa.Estado
                };
            output.IdRol = usuario.Id_Rol;
            output.IdSuperior = usuario.Id_Superior;
            output.DNI = usuario.DNI;
            output.Celular = usuario.Celular;
            output.Puesto = usuario.Puesto;

            return output;
        }
        public async Task<int> GetIdUsuario(string usuario_red)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => usuario_red.Equals(x.usuario_red) && x.Estado.Equals("ACT"));
            return usuario.Id_Usuario;
        }
        public async Task ActualizarUsuarios(List<Usuario> usuarios)
        {

            var lista_usuarios = await _context.Usuarios.ToListAsync();
            lista_usuarios.ForEach(x => x.Estado = "INA");
            var temp = new List<Entities.Usuario>();
            foreach (var usuario in usuarios)
            {
                var entidad = lista_usuarios.FirstOrDefault(x => x.usuario_red.Equals(usuario.UsuarioRed));
                if (entidad != null)
                {
                    entidad.Nombres = usuario.Nombres ?? "";
                    entidad.usuario_red = usuario.UsuarioRed ?? "";
                    entidad.Apellidos = usuario.Apellidos ?? "";
                    entidad.Email = usuario.Email ?? "";
                    entidad.Estado = "ACT";
                    entidad.DNI = usuario.DNI;
                    entidad.Celular = usuario.Celular;
                    entidad.Puesto = usuario.Puesto;
                    entidad.Id_Area_Trabajo = usuario.AreaTrabajo?.IdAreaTrabajo;
                    entidad.Id_Empresa = usuario.Empresa?.IdEmpresa;
                    temp.Add(entidad);
                }
                else
                {
                    entidad = new Entities.Usuario
                    {
                        Id_Rol = (int)InspeccionRol.Inspector,
                        Nombres = usuario.Nombres ?? "",
                        usuario_red = usuario.UsuarioRed ?? "",
                        Apellidos = usuario.Apellidos ?? "",
                        Email = usuario.Email ?? "",
                        Estado = "ACT",
                        DNI = usuario.DNI ?? "",
                        Celular = usuario.Celular ?? "",
                        Puesto = usuario.Puesto ?? "",
                        Id_Area_Trabajo = usuario.AreaTrabajo?.IdAreaTrabajo,
                        Id_Empresa = usuario.Empresa?.IdEmpresa
                    };
                    temp.Add(entidad);
                }
            }
            lista_usuarios.RemoveAll(x => temp.Select(y => y.Id_Usuario).Contains(x.Id_Usuario));
            lista_usuarios.AddRange(temp);
            _context.Usuarios.UpdateRange(lista_usuarios);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            var lista = await _context.Empresas.ToListAsync();
            var output = new List<Empresa>();
            foreach (var item in lista)
            {
                output.Add(new Empresa
                {
                    IdEmpresa = item.Id_Empresa,
                    RazonSocial = item.Razon_Social,
                    Estado = item.Estado
                });
            }
            return output;
        }

        public async Task<List<AreaTrabajo>> GetAreasTrabajo()
        {
            var lista = await _context.Areas_Trabajo.ToListAsync();
            var output = new List<AreaTrabajo>();
            foreach (var item in lista)
            {
                output.Add(new AreaTrabajo
                {
                    IdAreaTrabajo = item.Id_Area_Trabajo,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                });
            }
            return output;
        }

        public async Task<int> AgregarEmpresa(Empresa empresa)
        {
            var entidad = new Entities.Empresa
            {
                Razon_Social = empresa.RazonSocial,
                Estado = "ACT"
            };
            _context.Empresas.Add(entidad);
            await _context.SaveChangesAsync();

            return entidad.Id_Empresa;
        }

        public async Task<int> AgregarAreaTrabajo(AreaTrabajo area)
        {
            var entidad = new Entities.Area_Trabajo
            {
                Descripcion = area.Descripcion,
                Estado = "ACT"
            };
            _context.Areas_Trabajo.Add(entidad);
            await _context.SaveChangesAsync();

            return entidad.Id_Area_Trabajo;
        }
    }
}
