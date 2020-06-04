using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.GetEspecieFlora
{
    public class GetEspecieFlora : IGetEspecieFlora
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEspecieFlora(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<EspecieFloraOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetEspecie_Flora(id);
            var output = new EspecieFloraOutput(data);
            return output;
        }
    }
}
