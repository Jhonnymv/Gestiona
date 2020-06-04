using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Fauna.EditarEspecieFauna
{
    public interface IEditarEspecieFauna
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
