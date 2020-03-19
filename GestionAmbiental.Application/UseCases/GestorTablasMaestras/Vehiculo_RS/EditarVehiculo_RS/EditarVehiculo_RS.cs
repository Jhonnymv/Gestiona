using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.EditarVehiculo_RS
{
    public class EditarVehiculo_RS : IEditarVehiculo_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarVehiculo_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int Id_Vehiculo, string Placa, double Capacidad, string Estado)
        {
            var vehiculo = new Domain.ValueObjects.Vehiculo_RS();
            vehiculo.Id_Vehiculo_RS = Id_Vehiculo;
            vehiculo.Placa = Placa;
            vehiculo.Capacidad = Capacidad;
            vehiculo.Estado = Estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateVehiculo_RS(vehiculo);
        }
    }
}
