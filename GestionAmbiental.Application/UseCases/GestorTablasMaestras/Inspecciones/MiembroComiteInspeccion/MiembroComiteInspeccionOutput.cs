using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion
{
    public class MiembroComiteInspeccionOutput
    {
        public int IdMiembroComite { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Estado { get; set; }
        public MiembroComiteInspeccionOutput()
        {
        }
    }
}
