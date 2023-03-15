using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using GestionAmbiental.Application.Repositories.CompromisosMCB.GerenciaCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.GerenciaCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly IGerenciaReadOnlyRepository _gerenciaReadOnlyRepository;
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public NuevoRegistro(IGerenciaReadOnlyRepository gerenciaReadOnlyRepository, IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _gerenciaReadOnlyRepository = gerenciaReadOnlyRepository;
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository;
        }

        public async Task<int> Execute(string Gerencia, string Descripcion)
        {
            var existentes = await _getItemsReadOnlyRepository.GetGerencias();

            var existe = existentes.Find(x => x.Gerencia.Equals(Gerencia));

            if (existe != null)
            {
                return existe.Id;
            }
            else
            {
                var input = new Domain.CompromisosMCB.GerenciaCMCB
                {
                    Gerencia = Gerencia,
                    Descripcion = Descripcion

                };
                return await _gerenciaReadOnlyRepository.RegistrarGerencia(input);
            }
        }
    }
}
