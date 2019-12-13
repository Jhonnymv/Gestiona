using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.GestorTablasMaestras
{
    public class GestorTablasMaestrasRepository : IGestorTablasMaestrasReadOnlyRepository
    {
        private Context _context;

        public GestorTablasMaestrasRepository(Context context)
        {
            _context = context;
        }
               
        public async Task AddEmpresa(Empresa empresa)
        {
            var empresaEntity = new Entities.Empresa
            {
                Razon_Social = empresa.RazonSocial
            };
            _context.Empresas.Add(empresaEntity);
            await _context.SaveChangesAsync();
        }
               
        public async Task AddGerencia(Gerencia gerencia)
        {
            var gerenciaEntity = new Entities.Gerencia
            {
                Descripcion = gerencia.Descripcion
            };
            _context.Gerencias.Add(gerenciaEntity);
            await _context.SaveChangesAsync();
        }
               
        public async Task AddTipoCondicion(TipoCondicion tipoCondicion)
        {
            var tipoCondicionEntity = new Entities.Tipo_Condicion
            {
                Descripcion = tipoCondicion.Descripcion,
                Grupo = tipoCondicion.Grupo
            };
            _context.Tipos_Condicion.Add(tipoCondicionEntity);
            await _context.SaveChangesAsync();
        }
               
        public async Task<int> AddUsuario(Usuario usuario)
        {
            var usuarioEntity = new Entities.Usuario
            {
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                Id_Rol = usuario.IdRol,
                Id_Superior = usuario.IdSuperior.Value != 0 ? usuario.IdSuperior : null,
                usuario_red = usuario.UsuarioRed,
                Estado = "ACT"
            };
            _context.Usuarios.Add(usuarioEntity);
            await _context.SaveChangesAsync();
            return usuarioEntity.Id_Usuario;
        }
               
        public async Task<Empresa> GetEmpresa(int idEmpresa)
        {
            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(idEmpresa));
            var output = new Empresa
            {
                IdEmpresa = empresa.Id_Empresa,
                RazonSocial = empresa.Razon_Social
            };
            return output;            
        }
               
        public async Task<List<Empresa>> GetEmpresas()
        {
            var empresas = await _context.Empresas.ToListAsync();
            var output = new List<Empresa>();
            foreach (var item in empresas)
                output.Add(new Empresa(item.Id_Empresa, item.Razon_Social));
            return output;
        }
               
        public async Task<Gerencia> GetGerencia(int idGerencia)
        {
            var gerencia = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(idGerencia));
            var output = new Gerencia
            {
                IdGerencia = gerencia.Id_Gerencia,
                Descripcion = gerencia.Descripcion
            };
            return output;
        }
               
        public async Task<List<Gerencia>> GetGerencias()
        {
            var gerencias = await _context.Gerencias.ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in gerencias)
                output.Add(new Gerencia(item.Id_Gerencia, item.Descripcion));
            return output;
        }
               
        public async Task<TipoCondicion> GetTipoCondicion(int idTipoCondicion)
        {
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(idTipoCondicion));
            var output = new TipoCondicion
            {
                IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion,
                Descripcion = tipoCondicion.Descripcion,
                Grupo = tipoCondicion.Grupo
            };
            return output;
        }
               
        public async Task<List<TipoCondicion>> GetTiposCondicion()
        {
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            var output = new List<TipoCondicion>();
            foreach (var item in tiposCondicion)
                output.Add(new TipoCondicion(item.Id_Tipo_Condicion,item.Descripcion,item.Grupo));
            return output;
        }
               
        public async Task<Usuario> GetUsuario(int idUsuario)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(idUsuario));
            var output = new Usuario
            {
               Nombres = usuario.Nombres,
               Apellidos = usuario.Apellidos,
               Email = usuario.Email,
               IdRol = usuario.Id_Rol,
               UsuarioRed = usuario.usuario_red,
               IdSuperior = usuario.Id_Superior,
               Estado = usuario.Estado
            };
            return output;
        }
               
        public async Task<List<Usuario>> GetUsuarios(string filtro)
        {
            var usuarios = await _context.Usuarios.Where(x =>
            x.Nombres.Contains(filtro == null ? x.Nombres : filtro) ||
            x.Apellidos.Contains(filtro == null ? x.Apellidos : filtro)
            //x.Estado.Contains(filtro) ||
            //x.Email.Contains(filtro)
            ).OrderBy(x=>x.Nombres)
            .ToListAsync();
            
            var output = new List<Usuario>();
            foreach (var item in usuarios)
                output.Add(new Usuario(item.Id_Usuario,item.Id_Rol,item.usuario_red,item.Nombres,item.Apellidos,item.Email,item.Id_Superior,item.Estado));
            return output;
        }
               
        public async Task UpdateEmpresa(Empresa empresa)
        {
            var empresaEntity = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(empresa.IdEmpresa));
            empresaEntity.Razon_Social = empresa.RazonSocial;
            _context.Empresas.Update(empresaEntity);
            await _context.SaveChangesAsync();
        }
               
        public async Task UpdateGerencia(Gerencia gerencia)
        {
            var gerenciaEntity = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(gerencia.IdGerencia));
            gerenciaEntity.Descripcion = gerencia.Descripcion;
            _context.Gerencias.Update(gerenciaEntity);
            await _context.SaveChangesAsync();
        }
               
        public async Task UpdateTipoCondicion(TipoCondicion tipoCondicion)
        {
            var tipoCondicionEntity = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(tipoCondicion.IdTipoCondicion));
            tipoCondicionEntity.Descripcion = tipoCondicion.Descripcion;
            tipoCondicionEntity.Grupo = tipoCondicion.Grupo;
            _context.Tipos_Condicion.Update(tipoCondicionEntity);
            await _context.SaveChangesAsync();
        }
               
        public async Task UpdateUsuario(Usuario usuario)
        {
            var usuarioEntity = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(usuario.IdUsuario));
            usuarioEntity.Nombres = usuario.Nombres;
            usuarioEntity.Apellidos = usuario.Apellidos;
            usuarioEntity.Email = usuario.Email;
            usuarioEntity.Id_Rol = usuario.IdRol;
            usuarioEntity.Id_Superior = usuario.IdSuperior.Value != 0 ? usuario.IdSuperior : null;
            usuarioEntity.usuario_red = usuario.UsuarioRed;
            usuarioEntity.Estado = usuario.Estado;

            _context.Usuarios.Update(usuarioEntity);
            await _context.SaveChangesAsync();
        }
    }
}
