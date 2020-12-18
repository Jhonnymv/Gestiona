using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarPredecesores
{
    public interface IConsultarPredecesores
    {
        Task<List<DocumentoOutput>> Execute(int Id);
    }
}
