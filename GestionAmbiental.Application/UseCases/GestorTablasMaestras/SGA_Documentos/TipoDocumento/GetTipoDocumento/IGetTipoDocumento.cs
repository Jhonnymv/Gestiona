using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTipoDocumento
{
    public interface IGetTipoDocumento
    {
        Task<TipoDocumentoOutput> Execute(int IdTipoDocumento);
    }
}
