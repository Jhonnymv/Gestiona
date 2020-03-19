using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.AgregarVehiculo_RS
{
    public class AgregarVehiculo_RS : IAgregarVehiculo_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarVehiculo_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string placa, double capacidad)
        {
            var vehiculo = new Domain.ValueObjects.Vehiculo_RS
            {
                Placa = placa,
                Capacidad = capacidad
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddVehiculo_RS(vehiculo);
        }
    }
}
