using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.GetNivelIncidente
{
    public interface IGetNivelIncidente
    {
        Task<NivelIncidenteOutput> Execute(int id);
    }
}
