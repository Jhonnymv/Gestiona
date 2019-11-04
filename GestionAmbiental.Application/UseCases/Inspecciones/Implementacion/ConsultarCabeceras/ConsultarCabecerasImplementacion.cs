using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras
{
    public class ConsultarCabecerasImplementacion : IConsultarCabecerasImplementacion
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        public ConsultarCabecerasImplementacion(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<List<InspeccionCabecera>> Execute(int pagina, int fpp, DateTime fecha, int gerencia, string codigo)
        {
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraImplementacion(pagina, fpp, fecha, gerencia, codigo);
            return cabeceras;
        }
    }
}
