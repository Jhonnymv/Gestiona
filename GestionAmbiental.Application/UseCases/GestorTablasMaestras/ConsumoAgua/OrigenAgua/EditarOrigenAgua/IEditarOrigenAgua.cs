using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.EditarOrigenAgua
{
    public interface IEditarOrigenAgua
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
