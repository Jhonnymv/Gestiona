using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.GetOrigenAgua
{
    public interface IGetOrigenAgua
    {
        Task<OrigenAguaOutput> Execute(int id);
    }
}
