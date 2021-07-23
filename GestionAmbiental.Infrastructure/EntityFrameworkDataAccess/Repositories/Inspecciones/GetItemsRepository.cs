using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GestionAmbiental.Domain.Enum;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class GetItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;

        public GetItemsRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        
        public async Task<ItemsSelectResumen> GetItemsResumen()
        {
            var items = new ItemsSelectResumen();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            var gerenciasDomain = new List<Domain.ValueObjects.Gerencia>();
            foreach(var gerencia in gerencias)
            {
                gerenciasDomain.Add(new Domain.ValueObjects.Gerencia(gerencia.Id_Gerencia, gerencia.Descripcion));
            }
            var empresasContratistaDomain = new List<Domain.ValueObjects.Empresa>();
            foreach (var empresa in empresas)
            {
                empresasContratistaDomain.Add(new Domain.ValueObjects.Empresa(empresa.Id_Empresa, empresa.Razon_Social,empresa.Estado));
            }
            var tiposCondicionDomain = new List<Domain.ValueObjects.TipoCondicion>();
            foreach (var tipo in tiposCondicion)
            {
                tiposCondicionDomain.Add(new Domain.ValueObjects.TipoCondicion(tipo.Id_Tipo_Condicion,tipo.Descripcion,tipo.Grupo));
            }
            items.GerenciaEjecutora = gerenciasDomain;
            items.EmpContratMarcobre = empresasContratistaDomain;
            items.TipoCondicion = tiposCondicionDomain;
            return items;
        }
        public async Task<List<Empresa>> GetEmpresaContratistas()
        {
            var empresas = await _context.Empresas.ToListAsync();
            var output = new List<Empresa>();
            foreach(var item in empresas)
            {
                output.Add(new Empresa(item.Id_Empresa, item.Razon_Social, item.Estado));
            }
            return output;
        }
        public async Task<List<Gerencia>> GetGerencias()
        {
            var gerencias = await _context.Gerencias.ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in gerencias)
            {
                output.Add(new Gerencia(item.Id_Gerencia,item.Descripcion));
            }
            return output;
        }

        public async Task<List<TipoCondicion>> GetTiposCondicion()
        {
            var tipos = await _context.Tipos_Condicion.ToListAsync();
            var output = new List<TipoCondicion>();
            foreach (var item in tipos)
            {
                output.Add(new TipoCondicion(item.Id_Tipo_Condicion,item.Descripcion,item.Grupo));
            }
            return output;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.Where(x=>!x.Id_Rol.Equals((int)InspeccionRol.Admin) && x.Estado.Equals("ACT")&& (x.Id_Area_Trabajo!=null||x.Id_Empresa!=null)).ToListAsync();
            var output = new List<Usuario>();
            foreach (var item in usuarios)
            {
                output.Add(new Usuario(item.Id_Usuario,item.Id_Rol,item.Nombres,item.Apellidos,item.Email));
            }
            return output;
        }
    }
}
