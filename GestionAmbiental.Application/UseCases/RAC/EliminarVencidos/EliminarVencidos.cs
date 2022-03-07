using GestionAmbiental.Application.Repositories.RAC;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.EliminarVencidos
{
    public class EliminarVencidos : IEliminarVencidos
    {
        private readonly IGetRACReadOnlyRepository _getRACReadOnlyRepository;

        public EliminarVencidos(IGetRACReadOnlyRepository getRACReadOnlyRepository)
        {
            _getRACReadOnlyRepository = getRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getRACReadOnlyRepository));
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

            await _getRACReadOnlyRepository.EliminarVencidos(fInicio, fFinal);
        }
    }
}
