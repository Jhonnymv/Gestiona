using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarFormatoDocumento
{
    public interface IRegistrarFormatoDocumento
    {
        Task<int> Execute(int categoriaTipo, string nombreDoc, double versionDoc, string comentario, string fechaTentativa, string fechaAprobacion, string rutaPdf, string rutaEditable, int idElaborador);
    }
}
