using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.GetTiposInspeccion
{
    public interface IGetTiposInspeccion
    {
        Task<TiposInspeccionOutput> Execute(string filtro, int pagina, int rpp);
    }
}
