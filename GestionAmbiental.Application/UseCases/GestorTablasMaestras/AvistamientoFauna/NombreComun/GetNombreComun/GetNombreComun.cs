using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombreComun
{
    public class GetNombreComun : IGetNombreComun
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetNombreComun(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<NombreComunOutput> Execute(int idNombreComun)
        {
            var nombreComun = await _gestorTablasMaestrasReadOnlyRepository.GetNombreComun(idNombreComun);
            var output = new NombreComunOutput(nombreComun);
            return output;
        }
    }
}
