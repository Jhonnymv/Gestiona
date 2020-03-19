using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.EditarVehiculo_RS
{
    public interface IEditarVehiculo_RS
    {
        Task Execute(int Id_Vehiculo, string Placa, double Capacidad, string Estado);
    }
}
