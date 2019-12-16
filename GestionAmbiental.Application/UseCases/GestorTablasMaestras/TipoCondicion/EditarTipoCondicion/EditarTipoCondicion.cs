using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.EditarTipoCondicion
{
    public class EditarTipoCondicion : IEditarTipoCondicion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarTipoCondicion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idTipoCondición, string descripcion, string grupo)
        {
            var tipoCondicion = new Domain.ValueObjects.TipoCondicion();
            tipoCondicion.IdTipoCondicion = idTipoCondición;
            tipoCondicion.Descripcion = descripcion;
            tipoCondicion.Grupo = grupo;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateTipoCondicion(tipoCondicion);
        }
    }
}
