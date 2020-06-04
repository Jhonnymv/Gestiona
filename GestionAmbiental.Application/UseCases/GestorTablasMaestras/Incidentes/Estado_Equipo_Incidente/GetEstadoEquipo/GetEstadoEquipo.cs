using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Estado_Equipo_Incidente.GetEstadoEquipo
{
    public class GetEstadoEquipo : IGetEstadoEquipo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEstadoEquipo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<EstadoEquipoOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetEstado_Equipo_Incidente(id);
            var output = new EstadoEquipoOutput(data);
            return output;
        }
    }
}
