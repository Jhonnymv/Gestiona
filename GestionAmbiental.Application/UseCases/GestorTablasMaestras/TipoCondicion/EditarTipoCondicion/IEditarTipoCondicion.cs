    using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.EditarTipoCondicion
{
    public interface IEditarTipoCondicion
    {
        Task Execute(int idTipoCondición, string descripcion, string grupo);
    }
}
