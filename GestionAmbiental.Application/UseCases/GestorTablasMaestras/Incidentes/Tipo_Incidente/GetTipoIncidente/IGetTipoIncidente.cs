using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.GetTipoIncidente
{
    public interface IGetTipoIncidente
    {
        Task<TipoIncidenteOutput> Execute(int id);
    }
}
