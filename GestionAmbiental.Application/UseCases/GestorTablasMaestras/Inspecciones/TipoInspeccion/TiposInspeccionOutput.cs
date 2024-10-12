using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion
{
    public class TiposInspeccionOutput
    {
        public List<TipoInspeccionOutput> TiposInspeccion { get; set; }
        public int NTiposInspeccion { get; set; }
    }
}
