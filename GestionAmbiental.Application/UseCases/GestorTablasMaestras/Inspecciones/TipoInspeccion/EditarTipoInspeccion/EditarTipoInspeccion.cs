using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.EditarTipoInspeccion
{
    internal class EditarTipoInspeccion : IEditarTipoInspeccion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarTipoInspeccion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string descripcion)
        {
            var input = new Domain.ValueObjects.Tipo_Inspeccion();
            input.Id_Tipo_Inspeccion = id;
            input.Descripcion = descripcion;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateTipoInspeccion(input);
        }
    }
}
