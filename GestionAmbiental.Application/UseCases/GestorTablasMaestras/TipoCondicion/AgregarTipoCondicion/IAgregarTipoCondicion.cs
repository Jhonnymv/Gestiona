using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.AgregarTipoCondicion
{
    public interface IAgregarTipoCondicion
    {
        Task<int> Execute(string descripcion, string grupo);
    }
}
