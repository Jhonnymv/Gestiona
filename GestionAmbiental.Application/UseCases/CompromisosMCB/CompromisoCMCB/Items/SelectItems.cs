using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.Items
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
            var areas = await _getItemsReadOnlyRepository.GetAreas();
            var criterios = await _getItemsReadOnlyRepository.GetCriterios();
            var etapas = await _getItemsReadOnlyRepository.GetEtapas();
            var origenes = await _getItemsReadOnlyRepository.GetOrigenes();
            var autoridades=await _getItemsReadOnlyRepository.GetAutoridades();
            var aspectos=await _getItemsReadOnlyRepository.GetAspectos();
            var componetes=await _getItemsReadOnlyRepository.GetComponentes();
            var gerencias=await _getItemsReadOnlyRepository.GetGerencias();
            var usuarios=await _getItemsReadOnlyRepository.GetUsuarios();

            var output = new SelectItemsOutput();
            output.ItemsArea = areas;
            output.ItemsCriterio = criterios;
            output.ItemsEtapa = etapas;
            output.ItemsOrigen = origenes;
            output.ItemsAutoridad = autoridades;
            output.ItemsAspecto = aspectos;
            output.ItemsComponente = componetes;
            output.ItemsGerencia = gerencias;
            output.ItemsUsuario = usuarios;
            output.EstadosSustento = Enum.GetNames(typeof(EstadosSustento)).ToList();
            output.EstadosCompromiso = Enum.GetNames(typeof(EstadosCompromiso)).ToList();

            return output;
        }
    }
}
