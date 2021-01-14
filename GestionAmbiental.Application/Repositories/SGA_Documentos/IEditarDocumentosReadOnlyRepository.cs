using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.SGA_Documentos
{
    public interface IEditarDocumentosReadOnlyRepository
    {
        Task EditarRevisionDocumento(RevisionDocumento revision);
        Task EditarRevisionesDocumento(List<RevisionDocumento> revision, int IdTipo, int IdDocumento);
        Task EditarEstadoDocumento(Documento documento);
        Task ActivarPrimeraRevision(int IdDocumento);
        Task RechazarDocumento(int IdDocumento);
        Task EditarDocumento(Documento documento);
    }
}
