using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRSCierre
{
    public interface IConsultarManifiestoRSCierre
    {
        Task<ManifiestoRSOutput> Execute(int Id_Manifiesto, string usuario_red);
    }
}
