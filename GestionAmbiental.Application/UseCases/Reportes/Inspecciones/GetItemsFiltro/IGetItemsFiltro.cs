using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetItemsFiltro
{
    public interface IGetItemsFiltro
    {
        Task<ItemsFiltroOutput> Execute();
    }
}
