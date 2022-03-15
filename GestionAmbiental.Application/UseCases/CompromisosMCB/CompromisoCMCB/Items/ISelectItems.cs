using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.Items
{
    public interface ISelectItems
    {
        Task<SelectItemsOutput> Execute();
    }
}
