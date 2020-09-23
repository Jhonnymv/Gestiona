using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.ResolucionConsumoAgua.GetResolucion
{
    public interface IGetResolucion
    {
        Task<ResolucionConsumoAguaOutput> Execute(int id);
    }
}
