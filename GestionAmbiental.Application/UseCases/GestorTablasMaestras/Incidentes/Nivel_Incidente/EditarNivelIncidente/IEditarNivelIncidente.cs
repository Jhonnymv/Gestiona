using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.EditarNivelIncidente
{
    public interface IEditarNivelIncidente
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
