using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.SGA_Documentos.EditarDocumento
{
    public interface IEditarDocumento
    {
        Task Execute(int IdDoc, int IdTipoDoc, string codigo, string nombreDoc, double versionDoc, int? idPredecesor, string comentario, string rutaDoc, string usuarioRed, string opcion, List<RevisionAprobacionInputOutput> revisores, List<RevisionAprobacionInputOutput> aprobadores);
    }
}
