using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua
{
    public class OrigenesAguaOutput
    {
        public List<OrigenAguaOutput> Origenes { get; set; }
        public int NOrigenes { get; set; }
    }
}
