using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos
{
    public interface IConsultarDocumentos
    {
        Task<DocumentosOutput> Execute(string nombre, int tipo, string codigo, int condicion,int pagina, int rpp);
        Task<List<DocumentoOutput>> Execute();
        Task<List<DocumentoOutput>> Execute(int Id);
    }
}
