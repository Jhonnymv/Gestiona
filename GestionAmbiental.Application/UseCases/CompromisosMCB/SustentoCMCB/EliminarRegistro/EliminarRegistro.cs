using GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.EliminarRegistro
{
    public class EliminarRegistro: IEliminarRegistro
    {
        private readonly ISustentoReadOnlyRepository _sustentoReadOnlyRepository;

        public EliminarRegistro(ISustentoReadOnlyRepository sustentoReadOnlyRepository)
        {
            _sustentoReadOnlyRepository = sustentoReadOnlyRepository ?? throw new ArgumentNullException(nameof(sustentoReadOnlyRepository));
        }

        public async Task Execute(int Id)
        {
            await _sustentoReadOnlyRepository.EliminarSustento(Id);
        }
    }
}
