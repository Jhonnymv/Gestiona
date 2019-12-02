using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Inspecciones
{
    public interface IRegistarInspeccionReadOnlyRepository
    {
        Task<int> RegistrarInspeccionCabecera(NuevaInspeccionCabecera cabecera);
        Task<int> RegistrarInspeccionDetalle(NuevaInspeccionDetalle detalle,int Id_Usuario);
    }
}
