using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion
{
    public class MiembrosComiteInspeccionOutput
    {
        public List<MiembroComiteInspeccionOutput> MiembrosComiteInspeccion { get; set; }
        public int NMiembrosInspeccion { get; set; }
    }
}
