using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.EditarUnidadMedida
{
    public class EditarUnidadMedida : IEditarUnidadMedida
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarUnidadMedida(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string descripcion, string tipo, string estado)
        {
            var input = new Domain.Incidentes.Items.Unidad_Medida();
            input.Id_Unidad_Medida = id;
            input.Descripcion = descripcion;
            input.Tipo = tipo;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateUnidad_Medida(input);
        }
    }
}
