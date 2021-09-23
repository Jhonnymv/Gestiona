using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.EditarTipoFauna
{
    public class EditarTipoFauna : IEditarTipoFauna
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarTipoFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdTipoFauna, string Descripcion, string Foto, string Estado)
        {
            var tipoFauna = new Domain.AvistamientoFauna.Items.TipoFauna();
            tipoFauna.IdTipoFauna = IdTipoFauna;
            tipoFauna.Descripcion = Descripcion;
            tipoFauna.Foto = Foto;
            tipoFauna.Estado = Estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateTipoFauna(tipoFauna);
        }
    }
}
