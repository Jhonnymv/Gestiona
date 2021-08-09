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
        Task<InspeccionDetalle> GetInspeccionDetalle(int idDetalle);
        Task<List<InspeccionCabecera>> GetInspeccionesYObservaciones(int empresa, int gerencia, int respLevObs, int asesor, DateTime FechaInicio, DateTime FechaFin);
    }
}
