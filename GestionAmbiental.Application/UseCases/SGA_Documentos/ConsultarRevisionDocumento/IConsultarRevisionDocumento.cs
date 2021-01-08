using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarRevisionDocumento
{
    public interface IConsultarRevisionDocumento
    {
        Task<RevisionAprobacionInputOutput> Execute(int IdRevisionDocumento);
    }
}
