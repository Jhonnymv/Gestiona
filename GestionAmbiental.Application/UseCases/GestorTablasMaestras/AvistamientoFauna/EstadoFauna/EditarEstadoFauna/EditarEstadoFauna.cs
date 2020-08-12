using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.EditarEstadoFauna
{
    public class EditarEstadoFauna : IEditarEstadoFauna
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarEstadoFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idEstadoFauna, string descripcion, string estado)
        {
            var input = new Domain.AvistamientoFauna.Items.EstadoFauna();
            input.IdEstadoFauna = idEstadoFauna;
            input.Descripcion = descripcion;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateEstadoFauna(input);
        }
    }
}
