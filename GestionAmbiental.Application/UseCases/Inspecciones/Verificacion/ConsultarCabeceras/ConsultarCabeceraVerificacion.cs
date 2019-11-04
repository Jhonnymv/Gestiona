using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarCabeceras
{
    public class ConsultarCabeceraVerificacion : IConsultarCabeceraVerificacion
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        public ConsultarCabeceraVerificacion(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<InspeccionCabecera> Execute(int idCabecera)
        {
            var output = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraVerificacion(idCabecera);
            return output;
        }
    }
}
