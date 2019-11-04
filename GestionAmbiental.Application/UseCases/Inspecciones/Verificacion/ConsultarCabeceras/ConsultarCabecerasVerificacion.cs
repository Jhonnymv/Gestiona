using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Verificacion.ConsultarCabeceras
{
    public class ConsultarCabecerasVerificacion : IConsultarCabecerasVerificacion
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;

        public ConsultarCabecerasVerificacion(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(cabeceraInspeccionReadOnlyRepository));
        }

        public async Task<List<InspeccionCabecera>> Execute(int pagina, int fpp, DateTime fecha, int gerencia, string codigo)
        {
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraVerificacion(pagina, fpp, fecha, gerencia, codigo);
            return cabeceras;
        }
    }
}
