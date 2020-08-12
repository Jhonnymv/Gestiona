using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.GetTipoAvistamiento
{
    public class GetTipoAvistamiento : IGetTipoAvistamiento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTipoAvistamiento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TipoAvistamientoOutput> Execute(int idTipoAvistamiento)
        {
            var tipoAvistamiento = await _gestorTablasMaestrasReadOnlyRepository.GetTipoAvistamiento(idTipoAvistamiento);
            var output = new TipoAvistamientoOutput(tipoAvistamiento);
            return output;
        }
    }
}
