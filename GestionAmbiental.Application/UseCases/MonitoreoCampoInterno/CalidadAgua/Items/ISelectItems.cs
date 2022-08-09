using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.MonitoreoCampoInterno.CalidadAgua.Items
{
    public interface ISelectItems
    {
        Task<SelectItemsOuput> Execute();

    }
}
