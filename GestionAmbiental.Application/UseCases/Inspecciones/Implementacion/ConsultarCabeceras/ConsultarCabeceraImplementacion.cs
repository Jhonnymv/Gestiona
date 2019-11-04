using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras
{
    public class ConsultarCabeceraImplementacion : IConsultarCabeceraImplementacion
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        public ConsultarCabeceraImplementacion(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<InspeccionCabecera> Execute(int idCabecera)
        {
            var output = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraImplementacion(idCabecera);
            return output;
        }

    }
}
