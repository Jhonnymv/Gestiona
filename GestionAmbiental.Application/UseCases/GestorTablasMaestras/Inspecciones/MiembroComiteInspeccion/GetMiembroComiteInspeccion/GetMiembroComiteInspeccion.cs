using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.GetMiembroComiteInspeccion
{
    internal class GetMiembroComiteInspeccion : IGetMiembroComiteInspeccion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetMiembroComiteInspeccion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<MiembroComiteInspeccionOutput> Execute(int id)
        {
            var tipoInspeccion = await _gestorTablasMaestrasReadOnlyRepository.GetMiembroComiteInspeccion(id);
            var output = new MiembroComiteInspeccionOutput
            {
                IdMiembroComite = tipoInspeccion.Id_Miembro_Comite,
                Nombres = tipoInspeccion.Nombres,
                Apellidos = tipoInspeccion.Apellidos,
                Estado = tipoInspeccion.Estado,
            };
            return output;
        }
    }
}
