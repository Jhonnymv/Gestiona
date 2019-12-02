using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.ConsultarCabeceras
{
    public class ConsultarCabeceras : IConsultarCabeceras
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        public ConsultarCabeceras(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<InspeccionesCabeceraOutput> Execute(int pagina, int fpp, DateTime fecha, int gerencia, string codigo)
        {
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabecera(fecha, gerencia, codigo);
            var output = new InspeccionesCabeceraOutput();
            output.nCabeceras = cabeceras.Count;
            output.cabeceras = cabeceras.Skip((pagina - 1) * fpp).Take(fpp).ToList();
            return output;
        }
    }
}
