using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.GetUnidadMedida
{
    public class GetUnidadMedida : IGetUnidadMedida
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetUnidadMedida(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<UnidadMedidaOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetUnidad_Medida(id);
            var output = new UnidadMedidaOutput(data);
            return output;
        }
    }
}
