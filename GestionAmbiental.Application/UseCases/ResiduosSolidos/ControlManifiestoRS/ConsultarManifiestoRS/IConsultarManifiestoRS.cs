using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRS
{
    public interface IConsultarManifiestoRS
    {
        Task<ManifiestoRSOutput> Execute(int Id_Manifiesto, string usuario_red);
    }
}
