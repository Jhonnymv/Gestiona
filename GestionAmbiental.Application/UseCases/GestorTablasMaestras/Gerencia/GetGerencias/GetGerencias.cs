using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.GetGerencias
{
    public class GetGerencias : IGetGerencias
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetGerencias(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<GerenciasOutput> Execute(string filtro, int pagina, int rpp)
        {
            var empresas = await _gestorTablasMaestrasReadOnlyRepository.GetGerencias(filtro);

            var gerenciaOutputs = new List<GerenciaOutput>();
            foreach (var item in empresas)
            {
                var temp = new GerenciaOutput();
                temp.IdGerencia = item.IdGerencia;
                temp.Descripcion = item.Descripcion;
                gerenciaOutputs.Add(temp);
            }
            var output = new GerenciasOutput
            {
                nGerencias = gerenciaOutputs.Count,
                Gerencias = gerenciaOutputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
