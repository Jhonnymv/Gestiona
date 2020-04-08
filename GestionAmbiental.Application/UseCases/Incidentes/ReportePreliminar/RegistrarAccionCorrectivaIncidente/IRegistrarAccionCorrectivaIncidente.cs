using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarAccionCorrectivaIncidente
{
    public interface IRegistrarAccionCorrectivaIncidente
    {
        Task<int> Execute(int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado);
    }
}
