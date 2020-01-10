using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion
{
    public interface IConsultarInspeccionDetalle
    {
        Task<List<InspeccionDetalleOutput>> Execute(int idCabecera, string usuario_red);
        Task<List<InspeccionDetalleOutput>> ExecuteVerificacion(int idCabecera, string usuario_red);
        Task<List<InspeccionDetalleOutput>> ExecuteRegistro(int idCabecera,string usuario_red);
    }
}
