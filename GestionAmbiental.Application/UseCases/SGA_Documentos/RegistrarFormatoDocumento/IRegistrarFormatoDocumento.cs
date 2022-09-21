using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarFormatoDocumento
{
    public interface IRegistrarFormatoDocumento
    {
        Task<int> Execute(string? Codigo_Doc_Formato, int categoriaTipo, string nombreDoc, string versionDoc, string comentario, string fechaTentativa, string fechaAprobacion, string rutaPdf, string rutaEditable, int idElaborador);
    }
}
