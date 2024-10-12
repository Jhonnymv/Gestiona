using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion
{
    public class TipoInspeccionOutput
    {
        public int IdTipoInspeccion { get; set; }
        public string Descripcion { get; set; }
        public TipoInspeccionOutput()
        {
        }
    }
}
