using GestionAmbiental.Application.Repositories.GestorUsuarios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorUsuarios.ReasignarEmpresas
{
    public class ReasignarEmpresas: IReasignarEmpresas
    {
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ReasignarEmpresas(IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<string> Execute()
        {
            return await _gestorUsuarioReadOnlyRepository.ReasignarEmpresas();
        }
    }
}
