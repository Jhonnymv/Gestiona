using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora.Items
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
            var zonas = await _getItemsReadOnlyRepository.GetZonas();
            var nombresCientificos = await _getItemsReadOnlyRepository.GetNombrescientificos();
            var fenologias = await _getItemsReadOnlyRepository.GetFenologias();
            var vigores = await _getItemsReadOnlyRepository.GetVigores();
            var saluds=await _getItemsReadOnlyRepository.GetSaluds();
            var usos=await _getItemsReadOnlyRepository.GetUsos();
            var faunasAsociadas=await _getItemsReadOnlyRepository.GetfaunasAsociadas();
            //var usuarios=await _getItemsReadOnlyRepository.GetUsuarios();

            var output = new SelectItemsOutput();
            output.ItemsZona = zonas;
            output.ItemsNombreCientifico = nombresCientificos;
            output.ItemsFenologia = fenologias;
            output.ItemsVigor = vigores;
            output.ItemsSalud = saluds;
            output.ItemsUso = usos;
            output.ItemsFaunaAsociada = faunasAsociadas;
            //output.ItemsUsuario = usuarios;

            return output;
        }
    }
}
