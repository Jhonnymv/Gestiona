using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.GetTiposIncidente
{
    public interface IGetTiposIncidente
    {
        Task<TiposIncidenteOutput> Execute(string filtro, int pagina, int rpp);
    }
}
