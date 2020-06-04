using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.AgregarEspecieFlora
{
    public interface IAgregarEspecieFlora
    {
        Task<int> Execute(string descripcion);
    }
}
