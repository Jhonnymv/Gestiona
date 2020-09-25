using GestionAmbiental.Domain.ConsumoAgua;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.Items
{
    public class SelectItemsOutput
    {
        public List<ResolucionConsumoAgua> ItemsResoluciones { get; set; }
        public List<OrigenAgua> ItemsOrigenes { get; set; }
    }
}
