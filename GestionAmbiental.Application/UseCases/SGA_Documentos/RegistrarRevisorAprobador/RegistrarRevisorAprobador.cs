using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarRevisorAprobador
{
    public class RegistrarRevisorAprobador : IRegistrarRevisorAprovador
    {
        private readonly IRegistrarDocumentosReadOnlyRepository _registrarDocumentosReadOnlyRepository;

        public RegistrarRevisorAprobador(IRegistrarDocumentosReadOnlyRepository registrarDocumentosReadOnlyRepository)
        {
            _registrarDocumentosReadOnlyRepository = registrarDocumentosReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarDocumentosReadOnlyRepository));
        }

        public async Task<int> Execute(int idDocumento, int tipo, int orden, int idUsuario, string fechaLimite)
        {
            var model = new RevisionDocumento
            {
                IdSGADocumento = idDocumento,
                SGATipoRevision = new TipoRevision { IdTipoRevision = tipo },
                IdUsuario = idUsuario,
                Orden = orden,
                FechaLimite = Convert.ToDateTime(fechaLimite)
            };
            return await _registrarDocumentosReadOnlyRepository.RegistrarRevision(model);
        }
    }
}
