using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.Enum;

namespace GestionAmbiental.Application.UseCases.Inspecciones.SelectItems
{
    public class SelectItems : ISelectItems
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;
        public SelectItems(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }
        public async Task<ItemsSelectResumen> Execute()
        {
            var items = await _getItemsReadOnlyRepository.GetItemsResumen();
            return  items;
        }

        public async Task<ItemsSelectResumen> ItemVistaEditarInspeccion()
        {
            var empresa_Contratistas = await _getItemsReadOnlyRepository.GetEmpresaContratistas();
            var empresa_Contratistas_EPCM = await _getItemsReadOnlyRepository.GetEmpresaContratistasEPCM();
            var gerencias = await _getItemsReadOnlyRepository.GetGerencias();
            var gerencias_EPCM = await _getItemsReadOnlyRepository.GetGerenciasEPCM();
            var usuarios = await _getItemsReadOnlyRepository.GetUsuarios();
            var output = new ItemsSelectResumen();
            output.EmpContratMarcobre = empresa_Contratistas;
            output.EmpContratEPCM = empresa_Contratistas_EPCM;
            output.GerenciaEjecutora = gerencias;
            output.GerenciasEPCM = gerencias_EPCM;
            output.Usuarios = usuarios;
            return output;
        }

        public async Task<ItemsSelectResumen> ItemVistaNuevaInspeccion()
        {
            var empresa_Contratistas = await _getItemsReadOnlyRepository.GetEmpresaContratistas();
            var empresa_Contratistas_EPCM = await _getItemsReadOnlyRepository.GetEmpresaContratistasEPCM();
            var gerencias = await _getItemsReadOnlyRepository.GetGerencias();
            var gerencias_EPCM = await _getItemsReadOnlyRepository.GetGerenciasEPCM();
            var usuarios = await _getItemsReadOnlyRepository.GetUsuarios();
            usuarios = usuarios.Where(x => x.Id_Rol.Equals((int)InspeccionRol.Inspector)).ToList();
            var output = new ItemsSelectResumen();
            output.EmpContratMarcobre = empresa_Contratistas;
            output.EmpContratEPCM = empresa_Contratistas_EPCM;
            output.GerenciaEjecutora = gerencias;
            output.GerenciasEPCM = gerencias_EPCM;
            output.Usuarios = usuarios;
            return output;
        }

        public async Task<ItemsSelectResumen> ItemVistaNuevaObservacion()
        {
            var tipos = await _getItemsReadOnlyRepository.GetTiposCondicion();
            var usuarios = await _getItemsReadOnlyRepository.GetUsuarios();
            var output = new ItemsSelectResumen();
            output.TipoCondicion = tipos;
            output.Usuarios = usuarios;
            return output;
        }

        public async Task<ItemsSelectResumen> ItemVistaResumen()
        {
            var gerencias = await _getItemsReadOnlyRepository.GetGerencias();
            var output = new ItemsSelectResumen();
            output.GerenciaEjecutora = gerencias;
            return output;
        }
    }
}
