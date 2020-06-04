using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.AgregarTipoAgua
{
    public interface IAgregarTipoAgua
    {
        Task<int> Execute(string descripcion);
    }
}
