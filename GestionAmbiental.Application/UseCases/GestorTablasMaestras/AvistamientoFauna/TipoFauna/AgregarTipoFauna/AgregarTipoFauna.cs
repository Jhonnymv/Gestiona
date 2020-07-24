using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.AgregarTipoFauna
{
    public class AgregarTipoFauna : IAgregarTipoFauna
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarTipoFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string Descripcion)
        {
            var tipoFauna = new Domain.AvistamientoFauna.Items.TipoFauna()
            {
                Descripcion = Descripcion
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddTipoFauna(tipoFauna);
        }
    }
}
