using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.EditarTipoInspeccion
{
    public interface IEditarTipoInspeccion
    {
        Task Execute(int Id_Tipo_Inspeccion, string descripcion);
    }
}
