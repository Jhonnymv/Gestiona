using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.SelectItems
{
    public class SelectItems : ISelectItems
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public SelectItems(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }
        public async Task<SelectItemsOutput> Execute()
        {
            var empresas = await _getItemsReadOnlyRepository.GetEmpresas();
            var gerencias = await _getItemsReadOnlyRepository.GetGerencias();
            var usuarios = await _getItemsReadOnlyRepository.GetUsuarios();
            var tipos = await _getItemsReadOnlyRepository.GetTiposCondicion();
            var output = new SelectItemsOutput();
            output.ItemsEmpresa = empresas;
            output.ItemsGerencia = gerencias;
            output.ItemsUsuarios = usuarios;
            output.ItemsTiposCondicion = tipos;
            output.ItemsAsesoresMA = usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorMAP)).ToList();
            output.ItemsAsesoresSSO = usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorSSO)).ToList();
            return output;
        }
    }
}
