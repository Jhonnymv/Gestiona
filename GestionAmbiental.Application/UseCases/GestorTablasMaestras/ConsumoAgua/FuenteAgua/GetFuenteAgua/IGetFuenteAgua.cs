using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.GetFuenteAgua
{
    public interface IGetFuenteAgua
    {
        Task<FuenteAguaOutput> Execute(int id);
    }
}
