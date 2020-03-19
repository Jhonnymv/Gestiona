using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.GetVehiculo_RS
{
    public interface IGetVehiculo_RS
    {
        Task<Vehiculo_RSOutput> Execute(int idVehiculo);
    }
}
