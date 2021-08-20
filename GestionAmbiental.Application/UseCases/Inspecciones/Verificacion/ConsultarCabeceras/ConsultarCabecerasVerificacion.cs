using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<InspeccionesCabeceraOutput> Execute(int pagina, int fpp, DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo)
        {
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraVerificacion(fechaInicio, fechaFin, gerenciaFiltro, empresaFiltro, registradoPorFiltro, responsableFiltro, asesorFiltro,codigo);
            var output = new InspeccionesCabeceraOutput
            {
                NCabeceras = cabeceras.Count,
                Cabeceras = cabeceras.Skip((pagina - 1) * fpp).Take(fpp).ToList()
            };
            return output;
        }
    }
}
