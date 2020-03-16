using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ResiduosSolidos.ManifiestoRSRegistrado
{
    public interface IGetManifiestoRSRegistrado
    {
        Task<ManifiestoRSOutput> Execute(int Id_Manifiesto);
    }
}
