using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.AgregarTipoInspeccion
{
    public interface IAgregarTipoInspeccion
    {
        Task<int> Execute(string descripcion);
    }
}
