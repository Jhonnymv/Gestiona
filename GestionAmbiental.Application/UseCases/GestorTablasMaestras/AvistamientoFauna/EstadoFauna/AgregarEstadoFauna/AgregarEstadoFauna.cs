using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.AgregarEstadoFauna
{
    public class AgregarEstadoFauna : IAgregarEstadoFauna
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarEstadoFauna(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion)
        {
            var input = new Domain.AvistamientoFauna.Items.EstadoFauna()
            {
                Descripcion = descripcion
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddEstadoFauna(input);
        }
    }
}
