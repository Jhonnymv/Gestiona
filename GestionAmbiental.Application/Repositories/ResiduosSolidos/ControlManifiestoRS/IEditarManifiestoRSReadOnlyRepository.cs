using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS
{
    public interface IEditarManifiestoRSReadOnlyRepository
    {
        Task CompletarManifiestoRS(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS,int usuario);
        Task EditarManifiestoContratista(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS, int usuario);
        Task EliminarManifiesto(int IdManifiesto, int usuario);
    }
}
