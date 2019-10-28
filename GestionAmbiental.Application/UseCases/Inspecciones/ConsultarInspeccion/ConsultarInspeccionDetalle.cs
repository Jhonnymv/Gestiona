using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Newtonsoft.Json;

namespace GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccion
{
    public class ConsultarInspeccionDetalle : IConsultarInspeccionDetalle
    {
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnlyRepository;

        public ConsultarInspeccionDetalle(IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnlyRepository)
        {
            _detalleInspeccionReadOnlyRepository = detalleInspeccionReadOnlyRepository;
        }
        public async Task<List<InspeccionDetalleOutput>> Execute(int idCabecera)
        {
            var output = new List<InspeccionDetalleOutput>();
            var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(idCabecera);
            foreach(var item in observaciones)
            {
                output.Add(new InspeccionDetalleOutput(item));
            }
            return output;
        }
    }
}
