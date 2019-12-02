using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Inspecciones
{
    public class InspeccionesCabeceraOutput
    {
        public List<InspeccionCabecera> cabeceras { get; set; }
        public int nCabeceras { get; set; }
    }
}
