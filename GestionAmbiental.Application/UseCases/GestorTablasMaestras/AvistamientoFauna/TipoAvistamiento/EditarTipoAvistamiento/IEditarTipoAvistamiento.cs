using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.EditarTipoAvistamiento
{
    public interface IEditarTipoAvistamiento
    {
        Task Execute(int idTipoAvistamiento, string descripcion, string estado);
    }
}
