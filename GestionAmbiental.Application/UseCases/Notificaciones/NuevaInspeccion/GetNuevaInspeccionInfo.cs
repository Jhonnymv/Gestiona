using GestionAmbiental.Application.Repositories.Notificaciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Notificaciones.NuevaInspeccion
{
    public class GetNuevaInspeccionInfo : IGetNuevaInspeccionInfo
    {
        private readonly IGetInspeccionInfoReadOnlyRepository _getInspeccionInfoReadOnlyRepository;

        public GetNuevaInspeccionInfo(IGetInspeccionInfoReadOnlyRepository getInspeccionInfoReadOnlyRepository)
        {
            _getInspeccionInfoReadOnlyRepository = getInspeccionInfoReadOnlyRepository;
        }

        public async Task<InspeccionCabecera> Execute(int IdCabecera)
        {
            return await _getInspeccionInfoReadOnlyRepository.GetInfoCompletaInspeccion(IdCabecera);
        }
    }
}
