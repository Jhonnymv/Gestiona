using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.GetLugarDisposicion
{
    public class GetLugarDisposicion : IGetLugarDisposicion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetLugarDisposicion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<LugarDisposicionOutput> Execute(int id)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetLugar_Disposicion(id);
            var output = new LugarDisposicionOutput(data);
            return output;
        }
    }
}
