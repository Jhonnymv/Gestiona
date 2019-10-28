using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Inspecciones
{
    public interface IDetalleInspeccionReadOnlyRepository
    {
        Task<List<InspeccionDetalle>> GetInspeccionDetalles(int idCabecera);
    }
}
