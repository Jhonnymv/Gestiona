using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.Inspecciones
{
    public class GetItemsFiltroRepository : IGetItemsFiltroReadOnlyRepository
    {
        private Context _context;
        public GetItemsFiltroRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Usuario>> GetAsesoresMA()
        {
            var asesores = await _context.Usuarios.Where(x => x.Id_Rol.Equals((int)InspeccionRol.AsesorMAP) && x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Usuario>();
            foreach(var item in asesores)
            {
                output.Add(new Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }
            return output;
        }

        public async Task<List<Usuario>> GetAsesorSSO()
        {
            var asesores = await _context.Usuarios.Where(x => x.Id_Rol.Equals((int)InspeccionRol.AsesorSSO) && x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Usuario>();
            foreach (var item in asesores)
            {
                output.Add(new Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }
            return output;
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            var empresas = await _context.Empresas.ToListAsync();
            var output = new List<Empresa>();
            foreach (var item in empresas)
            {
                output.Add(new Empresa(item.Id_Empresa,item.Razon_Social));
            }
            return output;
        }

        public async Task<List<Gerencia>> GetGerencias()
        {
            var gerencias = await _context.Gerencias.ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in gerencias)
            {
                output.Add(new Gerencia{ IdGerencia = item.Id_Gerencia, Descripcion = item.Descripcion, Estado = item.Estado });
            }
            return output;
        }

        public async Task<List<Usuario>> GetRespLevObs()
        {
            var usuarios = await _context.Usuarios.Where(x => !x.Id_Rol.Equals((int)InspeccionRol.Admin) && x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Usuario>();
            foreach (var item in usuarios)
            {
                output.Add(new Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }
            return output;
        }
    }
}
