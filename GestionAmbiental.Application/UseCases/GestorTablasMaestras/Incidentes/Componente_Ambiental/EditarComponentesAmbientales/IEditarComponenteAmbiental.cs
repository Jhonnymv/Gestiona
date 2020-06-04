using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.EditarComponentesAmbientales
{
    public interface IEditarComponenteAmbiental
    {
        Task Execute(int idComponente, string descripcion, string estado);
    }
}
