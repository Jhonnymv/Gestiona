using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.GetLugarDisposicion
{
    public interface IGetLugarDisposicion
    {
        Task<LugarDisposicionOutput> Execute(int id);
    }
}
