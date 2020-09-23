using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua
{
    public class ResolucionesConsumoAguaOutput
    {
        public List<ResolucionConsumoAguaOutput> Resoluciones { get; set; }
        public int NResoluciones { get; set; }
    }
}
