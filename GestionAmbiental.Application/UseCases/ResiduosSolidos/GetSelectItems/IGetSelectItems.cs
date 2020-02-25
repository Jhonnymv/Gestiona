using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems
{
    public interface IGetSelectItems
    {
        Task<SelectItemsOutput> Execute();
    }
}
