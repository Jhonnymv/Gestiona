using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua
{
    public class FuentesAguaOutput
    {
        public List<FuenteAguaOutput> Fuentes { get; set; }
        public int NFuentes { get; set; }
    }
}
