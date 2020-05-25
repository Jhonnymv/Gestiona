using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.RegistrarAccion
{
    public interface IRegistrarAccionCorrectivaIncidenteInvestigacion
    {
        Task<int> Execute(int IdReporte,string tipoAccion, string descripcion, int usuario, string contratista, string fecha, string estado);
    }
}
