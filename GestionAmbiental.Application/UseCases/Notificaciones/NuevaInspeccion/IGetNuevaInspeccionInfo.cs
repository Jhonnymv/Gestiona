using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevaInspeccion
{
    public interface IGetNuevaInspeccionInfo
    {
        Task<InspeccionCabecera> Execute(int IdCabecera);
    }
}
