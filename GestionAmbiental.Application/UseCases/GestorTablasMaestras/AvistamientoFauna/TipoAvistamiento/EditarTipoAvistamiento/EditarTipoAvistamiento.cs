using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.EditarTipoAvistamiento
{
    public class EditarTipoAvistamiento : IEditarTipoAvistamiento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarTipoAvistamiento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idTipoAvistamiento, string descripcion, string estado)
        {
            var input = new Domain.AvistamientoFauna.Items.TipoAvistamiento();
            input.IdTipoAvistamiento = idTipoAvistamiento;
            input.Descripcion = descripcion;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateTipoAvistamiento(input);
        }
    }
}
