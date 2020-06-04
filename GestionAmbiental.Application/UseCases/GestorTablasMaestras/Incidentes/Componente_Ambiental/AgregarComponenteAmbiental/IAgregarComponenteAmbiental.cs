using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.AgregarComponenteAmbiental
{
    public interface IAgregarComponenteAmbiental
    {
        Task<int> Execute(string descripcion);
    }
}
