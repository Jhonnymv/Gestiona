using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.RAC
{
    public class GetItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;
        public GetItemsRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Empresa>> GetEmpresas()
        {
            var empresas = await _context.Empresas.ToListAsync();
            var output = new List<Empresa>();
            foreach(var item in empresas)
            {
                output.Add(new Empresa {
                    IdEmpresa = item.Id_Empresa, 
                    RazonSocial = item.Razon_Social,
                    Estado =item.Estado
                });
            }
            return output;
        }

        public async Task<List<Gerencia>> GetGerencias()
        {
            var gerencias = await _context.Gerencias.ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in gerencias)
            {
                output.Add(new Gerencia
                {
                    IdGerencia = item.Id_Gerencia,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                });
            }
            return output;
        }

        public async Task<List<TipoCondicion>> GetTiposCondicion()
        {
            var tipos = await _context.Tipos_Condicion.ToListAsync();
            var output = new List<TipoCondicion>();
            foreach (var item in tipos)
            {
                output.Add(new TipoCondicion
                {
                    IdTipoCondicion = item.Id_Tipo_Condicion,
                    Descripcion = item.Descripcion,
                    Grupo = item.Grupo
                });
            }
            return output;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.Where(x=>x.Estado!="INA" && x.Id_Rol!=(int)InspeccionRol.Admin && x.Estado.Equals("ACT") && (x.Id_Area_Trabajo != null || x.Id_Empresa != null)).ToListAsync();
            var output = new List<Usuario>();
            foreach (var item in usuarios)
            {
                output.Add(new Usuario
                {
                    IdUsuario = item.Id_Usuario,
                    IdRol = item.Id_Rol,
                    Nombres = item.Nombres,
                    Apellidos = item.Apellidos,
                    Email = item.Email
                });
            }
            return output;
        }
    }
}
