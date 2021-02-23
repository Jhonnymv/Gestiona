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
        Task RegistrarFormatos(List<int>idsFormato, int idDocumento);
        Task<string> GetUltimoCodigo();
        Task<int> RegistrarRevision(RevisionDocumento revision);
        Task<int> RegistrarRevisiones(List<RevisionDocumento> revisiones);
    }
}
