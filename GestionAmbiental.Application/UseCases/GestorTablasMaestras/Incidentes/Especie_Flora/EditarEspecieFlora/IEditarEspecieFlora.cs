using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.EditarEspecieFlora
{
    public interface IEditarEspecieFlora
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
