using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua
{
    public class ConsumosAguaOutput
    {
        public List<ConsumoAguaOutput> Consumos { get; set; }
        public int NConsumo { get; set; }
    }
}
