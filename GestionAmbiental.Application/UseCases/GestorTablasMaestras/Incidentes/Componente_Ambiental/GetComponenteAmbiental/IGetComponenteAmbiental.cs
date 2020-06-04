using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.GetComponenteAmbiental
{
    public interface IGetComponenteAmbiental
    {
        Task<ComponenteAmbientalOutput> Execute(int idComponente);
    }
}
