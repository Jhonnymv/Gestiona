using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.SGA_Documentos
{
    public interface IRegistrarDocumentosReadOnlyRepository
    {
        Task<int> RegistrarDocumento(Domain.SGA_Documentos.Documento documento);
        Task<string> GetUltimoCodigo();
    }
}
