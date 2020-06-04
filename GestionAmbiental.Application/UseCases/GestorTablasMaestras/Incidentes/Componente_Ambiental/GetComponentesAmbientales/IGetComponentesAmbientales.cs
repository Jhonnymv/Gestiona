using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.GetComponentesAmbientales
{
    public interface IGetComponentesAmbientales
    {
        Task<ComponentesAmbientalesOutput> Execute(string filtro, int pagina, int rpp);
    }
}
