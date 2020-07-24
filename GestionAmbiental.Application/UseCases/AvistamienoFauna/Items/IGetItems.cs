using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.Items
{
    public interface IGetItems
    {
        Task<ItemsOutput> Execute();
    }
}
