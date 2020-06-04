using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.AgregarEspecieFauna
{
    public interface IAgregarEspecieFauna
    {
        Task<int> Execute(string descripcion);
    }
}
