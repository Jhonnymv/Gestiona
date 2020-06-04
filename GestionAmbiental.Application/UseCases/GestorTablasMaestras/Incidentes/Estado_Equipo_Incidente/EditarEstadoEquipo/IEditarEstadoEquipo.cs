using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.EditarEstadoEquipo
{
    public interface IEditarEstadoEquipo
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
