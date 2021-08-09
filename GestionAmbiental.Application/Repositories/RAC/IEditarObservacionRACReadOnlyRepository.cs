using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.RAC
{
    public interface IEditarObservacionRACReadOnlyRepository
    {
        Task EditarRAC(Domain.RAC.RAC rac, int idUsuario);
        Task AmpliarFechaObservacionRAC(int idRAC, DateTime nuevaFecha,int idUsuario);
        Task EliminarRAC(int idRAC, int idUsuario);
    }
}
