using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.AgregarTipoDocumento
{
    public interface IAgregarTipoDocumento
    {
        Task<int> Execute(string Descripcion, string Codigo, string Formato);
    }
}
