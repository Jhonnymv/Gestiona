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

        public async Task<InspeccionesCabeceraOutput> Execute(int pagina, int fpp, DateTime fecha, int gerencia, string codigo)
        {
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraVerificacion(fecha, gerencia, codigo);
            var output = new InspeccionesCabeceraOutput();
            output.NCabeceras = cabeceras.Count;
            output.Cabeceras = cabeceras.Skip((pagina - 1) * fpp).Take(fpp).ToList();
            return output;
        }
    }
}
