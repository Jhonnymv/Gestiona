using GestionAmbiental.Domain.Inspecciones;
using System.Collections.Generic;

namespace GestionAmbiental.Application.UseCases.Inspecciones
{
    public class InspeccionesCabeceraOutput
    {
        public List<InspeccionCabecera> cabeceras { get; set; }
        public int nCabeceras { get; set; }
    }
}
