using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.EditarGerencia
{
    public class EditarGerencia : IEditarGerencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;
        public EditarGerencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idGerencia, string Descripcion,string Estado)
        {
            var gerencia = new Domain.ValueObjects.Gerencia
            {
                IdGerencia = idGerencia,
                Descripcion = Descripcion,
                Estado = Estado
            };
            await _gestorTablasMaestrasReadOnlyRepository.UpdateGerencia(gerencia);
        }
    }
}
