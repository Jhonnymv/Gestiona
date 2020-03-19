using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.GetVehiculos_RS
{
    public class GetVehiculos_RS : IGetVehiculos_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetVehiculos_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<Vehiculos_RSOutput> Execute(string filtro, int pagina, int rpp)
        {
            var vehiculos = await _gestorTablasMaestrasReadOnlyRepository.GetVehiculo_RS(filtro);

            var outputs = new List<Vehiculo_RSOutput>();
            foreach (var item in vehiculos)
            {
                var temp = new Vehiculo_RSOutput();
                temp.Id_Vehiculo_RS = item.Id_Vehiculo_RS;
                temp.Placa = item.Placa;
                temp.Capacidad = item.Capacidad;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new Vehiculos_RSOutput
            {
                NVehiculos = vehiculos.Count,
                Vehiculos = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
