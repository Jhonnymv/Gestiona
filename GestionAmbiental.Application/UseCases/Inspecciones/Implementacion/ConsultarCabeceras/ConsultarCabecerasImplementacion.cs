using GestionAmbiental.Application.Repositories.Inspecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras
{
    public class ConsultarCabecerasImplementacion : IConsultarCabecerasImplementacion
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        public ConsultarCabecerasImplementacion(
            ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<InspeccionesCabeceraOutput> Execute(int pagina, int fpp, DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro)
        {
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraImplementacion(fechaInicio, fechaFin, gerenciaFiltro, empresaFiltro, registradoPorFiltro, responsableFiltro, asesorFiltro);
            var output = new InspeccionesCabeceraOutput();
            output.NCabeceras = cabeceras.Count;
            output.Cabeceras = cabeceras.Skip((pagina - 1) * fpp).Take(fpp).ToList();
            return output;
        }
    }
}
