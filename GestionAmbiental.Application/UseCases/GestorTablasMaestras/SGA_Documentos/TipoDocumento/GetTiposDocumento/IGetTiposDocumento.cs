using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTiposDocumento
{
    public interface IGetTiposDocumento
    {
        Task<TiposDocumentoOutput> Execute(string filtro, int pagina, int rpp);
    }
}
