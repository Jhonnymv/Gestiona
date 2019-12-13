using GestionAmbiental.Domain.Inspecciones;
using System.Collections.Generic;

namespace GestionAmbiental.Application.UseCases.Inspecciones
{
    public class InspeccionesCabeceraOutput
    {
        public List<InspeccionCabecera> Cabeceras { get; set; }
        public int NCabeceras { get; set; }
    }
}
