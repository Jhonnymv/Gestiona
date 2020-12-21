using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.SGA_Documentos
{
    public interface IRegistrarDocumentosReadOnlyRepository
    {
        Task<int> RegistrarDocumento(Documento documento);
        Task<string> GetUltimoCodigo();
        Task<int> RegistrarRevisiones(List<RevisionDocumento> revisiones);
        Task<int> RegistrarAprobaciones(List<AprobacionDocumento> aprobaciones);
    }
}
