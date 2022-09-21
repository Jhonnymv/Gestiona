using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.RegistrarDocumento
{
    public interface IRegistrarDocumento
    {
        Task<int> Execute(int IdCategoriaTipo, string codigo, int IdDocPrincipal, string codigoTipoDoc, string nombreDoc, string versionDoc, int? idPredecesor, string comentario, string rutaPdf, string rutaEditable, string fechaTentativa,string fechaAprobacion, string? ElaboradoPorString, int? IdElaboradoPor, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores,List<int> secundarios);
    }
}
