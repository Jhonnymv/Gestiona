using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AreaTrabajo.AgregarAreaTrabajo
{
    public class AgregarAreaTrabajo : IAgregarAreaTrabajo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarAreaTrabajo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion)
        {
            var input = new Domain.ValueObjects.AreaTrabajo()
            {
                Descripcion = descripcion
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddAreaTrabajo(input);
        }
    }
}
