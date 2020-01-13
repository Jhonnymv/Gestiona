using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevoRAC
{
    public interface IGetNuevoRACInfo
    {
        Task<NuevoRACOutput> Execute(int IdRAC);
    }
}
