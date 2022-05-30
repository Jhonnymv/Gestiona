using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.FOCAM.Documentos.NuevoRegistro
{
    public interface INuevoRegistro
    {
        Task<int> Execute(
            int Id_Tipo,
            string Codigo,
            string Titulo,
            string Version,
            DateTime FechaAprobacion,
            string Estado,
            string? Comentarios,
            DateTime FechaTentativaVersion,
            string? UrlDocumento
            );
    }
}
