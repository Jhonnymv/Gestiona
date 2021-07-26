using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EliminarEvidenciaManifiestoRS
{
    public interface IEliminarEvidenciaManifiestoRS
    {
        Task Execute(int IdManifiesto, string Tipo, string NombreArchivo, string usuarioRed);
    }
}
