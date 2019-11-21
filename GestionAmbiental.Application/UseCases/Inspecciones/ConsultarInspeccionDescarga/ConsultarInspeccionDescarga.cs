using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccionDescarga
{
    public class ConsultarInspeccionDescarga : IConsultarInspeccionDescarga
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;

        public ConsultarInspeccionDescarga(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(cabeceraInspeccionReadOnlyRepository));
        }

        public async Task<InspeccionCabecera> Execute(int idCabecera)
        {
            var cabecera = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraDescarga(idCabecera);
            return cabecera;
        }
    }
}
