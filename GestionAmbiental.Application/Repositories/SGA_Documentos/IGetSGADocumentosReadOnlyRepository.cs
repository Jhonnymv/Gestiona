using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.SGA_Documentos
{
    public interface IGetSGADocumentosReadOnlyRepository
    {
        Task<List<Documento>> GetDocumentos(string nombre, int tipo, string codigo, int condicion );
        Task<Documento> GetDocumento(int Id);
        Task<List<Documento>> GetPredecesores(int Id);
        Task<RevisionDocumento> GetRevisionDocumento(int Id);
        Task<List<RevisionDocumento>> GetRevisionesDocumento(int IdDocumento);
    }
}
