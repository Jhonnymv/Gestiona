using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.ConsultarCabeceras
{
    public class ConsultarCabecera : IConsultarCabecera
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        public ConsultarCabecera(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<InspeccionCabecera> Execute(int idCabecera)
        {
            var output = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabecera(idCabecera);
            return output;
        }
    }
}
