using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.FOCAM.Documentos.ListarRegistros
{
    public interface IListarRegistros
    {
        Task<DocumentosOuput> Execute(int pagina, int rpp);
        Task<DocumentoOuput> BuscarById(int idDocumento);
    }
}
