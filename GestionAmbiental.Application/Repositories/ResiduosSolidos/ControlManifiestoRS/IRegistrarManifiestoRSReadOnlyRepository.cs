using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS
{
    public interface IRegistrarManifiestoRSReadOnlyRepository
    {
        Task<int> RegistrarManifiestosRS(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS);
    }
}
