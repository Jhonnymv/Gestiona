using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            var gerenciasDomain = new List<Domain.ValueObjects.Gerencia>();
            foreach(var gerencia in gerencias)
            {
                gerenciasDomain.Add(new Domain.ValueObjects.Gerencia(gerencia.Id_Gerencia, gerencia.Descripcion, gerencia.Email));
            }
            var empresasContratistaDomain = new List<Domain.ValueObjects.Empresa_Contratista>();
            var empresasEPCMDomain = new List<Domain.ValueObjects.Empresa_Contratista>();
            foreach (var empresa in empresas)
            {
                if(empresa.EPCM == 0)
                    empresasContratistaDomain.Add(new Domain.ValueObjects.Empresa_Contratista(empresa.Id_Empresa_Contratista, empresa.Razon_Social, empresa.EPCM));
                else
                    empresasEPCMDomain.Add(new Domain.ValueObjects.Empresa_Contratista(empresa.Id_Empresa_Contratista, empresa.Razon_Social, empresa.EPCM));

            }
            var tiposCondicionDomain = new List<Domain.ValueObjects.Tipo_Condicion>();
            foreach (var tipo in tiposCondicion)
            {
                tiposCondicionDomain.Add(new Domain.ValueObjects.Tipo_Condicion(tipo.Id_Tipo_Condicion,tipo.Descripcion,tipo.Grupo));
            }
            items.GerenciaEjecutora = gerenciasDomain;
            items.GerenciasEPCM = gerenciasDomain;
            items.EmpContratMarcobre = empresasContratistaDomain;
            items.EmpContratEPCM = empresasEPCMDomain;
            items.TipoCondicion = tiposCondicionDomain;
            return items;
        }
        public async Task<List<Empresa_Contratista>> GetEmpresaContratistas()
        {
            var empresas = await _context.Empresas_Contratistas.Where(x => x.EPCM.Equals(0)).ToListAsync();
            var output = new List<Empresa_Contratista>();
            foreach(var item in empresas)
            {
                output.Add(new Empresa_Contratista(item.Id_Empresa_Contratista, item.Razon_Social, item.EPCM));
            }
            return output;
        }

        public async Task<List<Empresa_Contratista>> GetEmpresaContratistasEPCM()
        {
            var empresas = await _context.Empresas_Contratistas.Where(x=>x.EPCM.Equals(1)).ToListAsync();
            var output = new List<Empresa_Contratista>();
            foreach (var item in empresas)
            {
                output.Add(new Empresa_Contratista(item.Id_Empresa_Contratista, item.Razon_Social, item.EPCM));
            }
            return output;
        }

        public async Task<List<Gerencia>> GetGerencias()
        {
            var gerencias = await _context.Gerencias.ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in gerencias)
            {
                output.Add(new Gerencia(item.Id_Gerencia,item.Descripcion,item.Email));
            }
            return output;
        }

        public async Task<List<Gerencia>> GetGerenciasEPCM()
        {
            var gerencias = await _context.Gerencias.ToListAsync();
            var output = new List<Gerencia>();
            foreach (var item in gerencias)
            {
                output.Add(new Gerencia(item.Id_Gerencia, item.Descripcion, item.Email));
            }
            return output;
        }

        public async Task<List<Tipo_Condicion>> GetTiposCondicion()
        {
            var tipos = await _context.Tipos_Condicion.ToListAsync();
            var output = new List<Tipo_Condicion>();
            foreach (var item in tipos)
            {
                output.Add(new Tipo_Condicion(item.Id_Tipo_Condicion,item.Descripcion,item.Grupo));
            }
            return output;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            var output = new List<Usuario>();
            foreach (var item in usuarios)
            {
                output.Add(new Usuario(item.Id_Usuario,item.Id_Rol,item.Nombres,item.Apellidos,item.Email));
            }
            return output;
        }
    }
}
