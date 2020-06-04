using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.EditarTipoAgua
{
    public interface IEditarTipoAgua
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
