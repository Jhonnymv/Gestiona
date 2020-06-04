using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Agua.EditarTipoAgua
{
    public class EditarTipoAgua : IEditarTipoAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarTipoAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string descripcion, string estado)
        {
            var input = new Domain.Incidentes.Items.Tipo_Agua();
            input.Id_Tipo_Agua = id;
            input.Descripcion = descripcion;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateTipo_Agua(input);
        }
    }
}
