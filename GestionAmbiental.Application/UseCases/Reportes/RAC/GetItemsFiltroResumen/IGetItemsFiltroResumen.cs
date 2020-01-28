using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetItemsFiltroResumen
{
    public interface IGetItemsFiltroResumen
    {
        Task<ItemsFiltroResumenOutput> Execute();
    }
}
