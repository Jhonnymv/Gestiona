using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumoAgua
{
    public interface IGetConsumoAgua
    {
        Task<ConsumoAguaOutput> Execute(int Id);
    }
}
