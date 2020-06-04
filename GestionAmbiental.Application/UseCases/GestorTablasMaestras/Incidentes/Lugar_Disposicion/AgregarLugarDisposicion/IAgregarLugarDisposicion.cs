using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.AgregarLugarDisposicion
{
    public interface IAgregarLugarDisposicion
    {
        Task<int> Execute(string descripcion);
    }
}
