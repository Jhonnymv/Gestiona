using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EditarAccion
{
    public interface IEditarAccionIncidente
    {
        //Task<int> Execute(int IdAccion, int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado);
        Task<int> Execute(int IdAccion, int IdReporte, string Estatus);
        Task<int> Execute(int IdAccion, int IdReporte, string tipoAccion, int IdUsuario, int IdUsuario2, string AccionInmediata, string Fecha, string Responsable, string Estatus);
    }
}
