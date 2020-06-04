using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.AgregarUnidadMedida
{
    public interface IAgregarUnidadMedida
    {
        Task<int> Execute(string descripcion, string tipo);
    }
}
