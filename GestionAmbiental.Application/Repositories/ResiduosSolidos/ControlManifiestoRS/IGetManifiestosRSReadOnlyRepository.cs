using GestionAmbiental.Domain.ResiduosSolidos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS
{
    public interface IGetManifiestosRSReadOnlyRepository
    {
        Task<List<Manifiesto_RS>> GetManifiestosRS(string nManifiesto, DateTime? fechaEmision, string observacion, int residuoSolido, int eors_salida, int eors_llegada, int asesorMAP);
        Task<Manifiesto_RS> GetManifiestoRS(int IdManifiestoRS);
    }
}
