using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.EditarDocumento
{
    public interface IEditarDocumento
    {
        Task Execute(int IdDoc, int IdCategoria, int IdTipoDoc, string codigo, string codigoTipoDoc, string nombreDoc, double versionDoc, int? idPredecesor, string comentario, string rutaDoc, string fechaTentativa, string usuarioRed, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores, List<int> secundarios);
    }
}
