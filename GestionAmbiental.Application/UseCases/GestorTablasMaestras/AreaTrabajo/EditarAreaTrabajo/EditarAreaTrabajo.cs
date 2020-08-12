using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.EditarAreaTrabajo
{
    public class EditarAreaTrabajo : IEditarAreaTrabajo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarAreaTrabajo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idAreaTrabajo, string descripcion, string estado)
        {
            var input = new Domain.ValueObjects.AreaTrabajo();
            input.IdAreaTrabajo = idAreaTrabajo;
            input.Descripcion = descripcion;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateAreaTrabajo(input);
        }
    }
}
