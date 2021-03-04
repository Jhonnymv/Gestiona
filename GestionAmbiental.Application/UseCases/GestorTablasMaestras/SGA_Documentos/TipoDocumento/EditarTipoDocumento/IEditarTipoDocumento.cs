using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.EditarTipoDocumento
{
    public interface IEditarTipoDocumento
    {
        Task Execute(int IdTipoDocumento, string Descripcion, string Codigo, string Formato, string Estado);
    }
}
