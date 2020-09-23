using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.AgregarOrigenAgua
{
    public interface IAgregarOrigenAgua
    {
        Task<int> Execute(string Descripcion);
    }
}
