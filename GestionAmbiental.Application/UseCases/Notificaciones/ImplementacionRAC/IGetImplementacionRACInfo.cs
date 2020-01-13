using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.ImplementacionRAC
{
    public interface IGetImplementacionRACInfo
    {
        Task<ImplementacionRACOutput> Execute(int Id_RAC);
    }
}
