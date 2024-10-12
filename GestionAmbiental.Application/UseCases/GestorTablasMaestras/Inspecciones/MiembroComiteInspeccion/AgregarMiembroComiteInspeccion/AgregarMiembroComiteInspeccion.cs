using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.AgregarMiembroComiteInspeccion
{
    internal class AgregarMiembroComiteInspeccion : IAgregarMiembroComiteInspeccion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarMiembroComiteInspeccion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string nombre, string apellido)
        {
            var input = new Domain.ValueObjects.MiembroComiteInspeccion()
            {
                Nombres = nombre,
                Apellidos = apellido,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddMiembroComiteInspeccion(input);
        }
    }
}
