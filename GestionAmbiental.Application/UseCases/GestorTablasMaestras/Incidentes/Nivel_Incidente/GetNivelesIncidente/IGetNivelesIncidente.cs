using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.GetNivelesIncidente
{
    public interface IGetNivelesIncidente
    {
        Task<NivelesIncidenteOutput> Execute(string filtro, int pagina, int rpp);
    }
}
