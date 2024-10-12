using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.GetTipoInspeccion
{
    internal class GetTipoInspeccion : IGetTipoInspeccion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTipoInspeccion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TipoInspeccionOutput> Execute(int id)
        {
            var tipoInspeccion = await _gestorTablasMaestrasReadOnlyRepository.GetTipoInspeccion(id);
            var output = new TipoInspeccionOutput{
                IdTipoInspeccion = tipoInspeccion.Id_Tipo_Inspeccion,
                Descripcion = tipoInspeccion.Descripcion
            };
            return output;
        }
    }
}
