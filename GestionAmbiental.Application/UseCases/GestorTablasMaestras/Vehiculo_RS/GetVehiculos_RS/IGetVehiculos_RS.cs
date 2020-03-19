using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.GetVehiculos_RS
{
    public interface IGetVehiculos_RS
    {
        Task<Vehiculos_RSOutput> Execute(string filtro, int pagina, int rpp);
    }
}
