using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.EditarMiembroComiteInspeccion
{
    internal class EditarMiembroComiteInspeccion : IEditarMiembroComiteInspeccion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarMiembroComiteInspeccion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int Id, string Nombres, string Apellidos, string Estado)
        {
            var input = new Domain.ValueObjects.MiembroComiteInspeccion();
            input.Id_Miembro_Comite = Id;
            input.Nombres = Nombres;
            input.Apellidos = Apellidos;
            input.Estado = Estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateMiembroComiteInspeccion(input);
        }
    }
}
