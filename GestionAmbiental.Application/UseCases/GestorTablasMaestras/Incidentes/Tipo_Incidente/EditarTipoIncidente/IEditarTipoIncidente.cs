using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Incidente.EditarTipoIncidente
{
    public interface IEditarTipoIncidente
    {
        Task Execute(int id, string descripcion, string estado);
    }
}
