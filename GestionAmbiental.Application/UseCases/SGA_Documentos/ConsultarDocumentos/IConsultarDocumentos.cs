using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumentos
{
    public interface IConsultarDocumentos
    {
        Task<DocumentosOutput> Execute(string nombre, int categoria, int[] tipos, string codigo, int condicion,int pagina, int rpp);
        Task<DocumentosOutput> Execute(string nombre, int categoria, int[] tipos, string codigo, int[] condicion,int pagina, int rpp);
        Task<List<DocumentoOutput>> Execute();
        Task<List<DocumentoOutput>> ExecutePredecesores(int Id);
        Task<List<DocumentoOutput>> ExecuteSecundarios(int Id);
    }
}
