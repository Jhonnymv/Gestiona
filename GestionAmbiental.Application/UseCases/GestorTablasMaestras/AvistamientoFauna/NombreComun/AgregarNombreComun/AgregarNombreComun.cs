using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.AgregarNombreComun
{
    public class AgregarNombreComun : IAgregarNombreComun
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarNombreComun(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string Descripcion, int IdTipoFauna, string Foto)
        {
            var nombreComun = new Domain.AvistamientoFauna.Items.NombreComunFauna()
            {
                Descripcion = Descripcion,
                TipoFauna = new Domain.AvistamientoFauna.Items.TipoFauna { IdTipoFauna = IdTipoFauna },
                Foto = Foto
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddNombreComun(nombreComun);
        }
    }
}
