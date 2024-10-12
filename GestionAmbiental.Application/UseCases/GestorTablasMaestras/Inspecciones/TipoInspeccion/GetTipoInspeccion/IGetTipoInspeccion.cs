using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.GetTipoInspeccion
{
    public interface IGetTipoInspeccion
    {
        Task<TipoInspeccionOutput> Execute(int id);
    }
}
