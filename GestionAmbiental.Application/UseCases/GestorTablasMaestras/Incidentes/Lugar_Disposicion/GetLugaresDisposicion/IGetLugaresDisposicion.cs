using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.GetLugaresDisposicion
{
    public interface IGetLugaresDisposicion
    {
        Task<LugaresDisposicionOutput> Execute(string filtro, int pagina, int rpp);
    }
}
