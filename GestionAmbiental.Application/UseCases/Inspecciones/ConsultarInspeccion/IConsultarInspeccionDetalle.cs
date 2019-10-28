
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccion
{
    public interface IConsultarInspeccionDetalle
    {
        Task<List<InspeccionDetalleOutput>> Execute(int idCabecera);
    }
}
