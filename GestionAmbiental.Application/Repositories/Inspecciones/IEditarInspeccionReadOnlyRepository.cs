using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Inspecciones
{
    public interface IEditarInspeccionReadOnlyRepository
    {
        Task EditarInspeccionCabecera(NuevaInspeccionCabecera cabecera,int idUsuario);
        Task EditarInspeccionDetalle(NuevaInspeccionDetalle detalle, int idUsuario);
        Task EliminarInspeccionDetalle(int IdObservacion, int idUsuario);
        Task<string> GetUltimoCodigo();
    }
}
