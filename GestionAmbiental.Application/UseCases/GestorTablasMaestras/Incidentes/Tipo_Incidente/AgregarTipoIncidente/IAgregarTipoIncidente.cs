using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.AgregarTipoIncidente
{
    public interface IAgregarTipoIncidente
    {
        Task<int> Execute(string descripcion);
    }
}
