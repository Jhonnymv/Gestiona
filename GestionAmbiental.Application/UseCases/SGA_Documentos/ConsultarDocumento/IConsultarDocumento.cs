using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumento
{
    public interface IConsultarDocumento
    {
        Task<DocumentoOutput> Execute(int IdDocumento);
    }
}
