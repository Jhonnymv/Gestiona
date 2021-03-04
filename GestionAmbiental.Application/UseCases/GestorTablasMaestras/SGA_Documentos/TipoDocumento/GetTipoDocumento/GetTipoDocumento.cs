using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTipoDocumento
{
    public class GetTipoDocumento : IGetTipoDocumento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetTipoDocumento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<TipoDocumentoOutput> Execute(int IdTipoDocumento)
        {
            var tipoDocumento = await _gestorTablasMaestrasReadOnlyRepository.GetTipoDocumento(IdTipoDocumento);
            var output = new TipoDocumentoOutput(tipoDocumento);
            return output;
        }
    }
}
