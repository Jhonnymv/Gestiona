using GestionAmbiental.Application.Repositories.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.EliminarVencidos
{
    public class EliminarVencidos : IEliminarVencidos
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;

        public EliminarVencidos(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(cabeceraInspeccionReadOnlyRepository));
        }

        public async Task Execute(string fechaInicio, string fechaFin)
        {
            DateTime fInicio, fFinal;
            if (fechaInicio != null)
                fInicio = Convert.ToDateTime(fechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (fechaFin != null)
                fFinal = Convert.ToDateTime(fechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;

            await _cabeceraInspeccionReadOnlyRepository.EliminarVencidos(fInicio, fFinal);
        }
    }
}
