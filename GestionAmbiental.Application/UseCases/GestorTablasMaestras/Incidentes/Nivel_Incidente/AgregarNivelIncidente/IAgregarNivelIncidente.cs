using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Nivel_Incidente.AgregarNivelIncidente
{
    public interface IAgregarNivelIncidente
    {
        Task<int> Execute(string descripcion);
    }
}
