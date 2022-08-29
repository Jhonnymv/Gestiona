using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.EditarDocumento
{
    public interface IEditarDocumento
    {
        Task Execute(int IdDoc, int IdCategoria, int IdCategoriaTipo, string codigo, string codigoTipoDoc, string nombreDoc, string versionDoc, int? idPredecesor, string comentario, string rutaPdf, string rutaEditable, string fechaTentativa, string fechaAprobacion, int IdElaboradoPor, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores, List<int> secundarios);
    }
}
