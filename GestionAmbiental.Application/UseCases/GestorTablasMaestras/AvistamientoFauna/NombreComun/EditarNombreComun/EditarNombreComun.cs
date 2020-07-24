using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.EditarNombreComun
{
    public class EditarNombreComun : IEditarNombreComun
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarNombreComun(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdNombreComun, string Descripcion, int IdTipoFauna, string Foto, string Estado)
        {
            var nombreComun = new Domain.AvistamientoFauna.Items.NombreComunFauna();
            nombreComun.IdNombreComunFauna = IdNombreComun;
            nombreComun.TipoFauna = new Domain.AvistamientoFauna.Items.TipoFauna{
                IdTipoFauna = IdTipoFauna
            };
            nombreComun.Descripcion = Descripcion;
            nombreComun.Foto = Foto;
            nombreComun.Estado = Estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateNombreComun(nombreComun);
        }
    }
}
