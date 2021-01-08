using GestionAmbiental.Application.Repositories.SGA_Documentos;
using GestionAmbiental.Domain.SGA_Documentos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.SGA_Documentos
{
    public class EditarDocumentosRepository : IEditarDocumentosReadOnlyRepository
    {
        public Task EditarRevisionDocumento(RevisionDocumento revision)
        {
            throw new NotImplementedException();
        }
    }
}
