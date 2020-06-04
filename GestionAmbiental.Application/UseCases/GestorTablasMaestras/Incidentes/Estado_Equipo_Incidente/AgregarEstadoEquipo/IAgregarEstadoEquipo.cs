using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.AgregarEstadoEquipo
{
    public interface IAgregarEstadoEquipo
    {
        Task<int> Execute(string descripcion);
    }
}
