using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.GetEstadosEquipos
{
    public interface IGetEstadosEquipos
    {
        Task<EstadosEquiposOutput> Execute(string filtro, int pagina, int rpp);
    }
}
