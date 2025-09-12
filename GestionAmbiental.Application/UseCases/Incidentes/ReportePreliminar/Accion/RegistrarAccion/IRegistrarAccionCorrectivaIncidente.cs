using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.RegistrarAccion
{
    public interface IRegistrarAccionCorrectivaIncidente
    {
        Task<int> Execute(int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado);
        Task<int> Execute(int IdReporte, string tipoAccion, string descripcion, int usuario, int usuario2, string contratista, string fecha, string estado);
    }
}
