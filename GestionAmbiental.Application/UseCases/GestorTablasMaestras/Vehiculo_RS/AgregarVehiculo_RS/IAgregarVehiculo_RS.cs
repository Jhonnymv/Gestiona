using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.AgregarVehiculo_RS
{
    public interface IAgregarVehiculo_RS
    {
        Task<int> Execute(string placa,double capacidad);
    }
}
