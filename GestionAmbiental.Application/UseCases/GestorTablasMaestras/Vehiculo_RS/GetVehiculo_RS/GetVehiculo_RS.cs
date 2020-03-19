using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.GetVehiculo_RS
{
    public class GetVehiculo_RS : IGetVehiculo_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetVehiculo_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<Vehiculo_RSOutput> Execute(int idVehiculo)
        {
            var vehiculo = await _gestorTablasMaestrasReadOnlyRepository.GetVehiculo_RS(idVehiculo);
            var output = new Vehiculo_RSOutput(vehiculo);
            return output;
        }
    }
}
