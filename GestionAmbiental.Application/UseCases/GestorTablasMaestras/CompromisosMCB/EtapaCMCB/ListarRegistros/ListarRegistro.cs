using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.ListarRegistros
{
    public class ListarRegistro : IListarRegistro
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public ListarRegistro(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<EtapasOuput> Execute(string filtro, int pagina, int rpp)
        {
            var etapas = await _getItemsReadOnlyRepository.GetEtapas();
            var outputs = new List<EtapaOuput>();
            foreach (var item in etapas)
            {
                var temp = new EtapaOuput();
                temp.Id = item.Id;
                temp.Etapa = item.Etapa;
                outputs.Add(temp);
            }
            var output = new EtapasOuput
            {
                NRegistros = etapas.Count,
                Registros = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}