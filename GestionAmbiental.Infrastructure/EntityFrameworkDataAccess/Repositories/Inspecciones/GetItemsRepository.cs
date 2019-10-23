using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                tiposCondicionDomain.Add(new Domain.ValueObjects.Tipo_Condicion(tipo.Id_Tipo_Condicion,tipo.Descripcion));
            }
            items.GerenciaEjecutora = gerenciasDomain;
            items.GerenciasEPCM = gerenciasDomain;
            items.EmpContratMarcobre = empresasContratistaDomain;
            items.EmpContratEPCM = empresasEPCMDomain;
            items.TipoCondicion = tiposCondicionDomain;
            return items;
        }
    }
}
