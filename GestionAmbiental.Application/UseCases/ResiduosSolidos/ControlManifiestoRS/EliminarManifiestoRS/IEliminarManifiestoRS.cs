using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.EliminarManifiestoRS
{
    public interface IEliminarManifiestoRS
    {
        Task Execute(int IdManifiestoRS, string usuario_red);
    }
}
