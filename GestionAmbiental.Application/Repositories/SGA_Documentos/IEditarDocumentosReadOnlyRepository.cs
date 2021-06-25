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
        //Task ActivarPrimeraRevision(int IdDocumento);
        //Task RechazarDocumento(int IdDocumento);
        Task EditarDocumento(Documento documento); 
        Task<string> GetUltimoCodigo(int IdCategoriaTipo);
        Task FirmarDocumento(Documento documento);
        Task EditarDocumentosRelacionados(List<int> secundarios, int idDocumento);
        Task EditarDocumentosSecundarios(List<Documento> secundarios);
        //Task AmpliarFechaRevision(RevisionDocumento revision);
    }
}
