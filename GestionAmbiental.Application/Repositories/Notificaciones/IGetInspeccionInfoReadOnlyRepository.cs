using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Notificaciones
{
    public interface IGetInspeccionInfoReadOnlyRepository
    {
        Task<InspeccionCabecera> GetInfoCompletaInspeccion(int idCabecera);
    }
}
