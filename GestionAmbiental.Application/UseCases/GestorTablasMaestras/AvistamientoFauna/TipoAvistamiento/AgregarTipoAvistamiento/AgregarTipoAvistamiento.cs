using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.AgregarTipoAvistamiento
{
    public class AgregarTipoAvistamiento : IAgregarTipoAvistamiento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarTipoAvistamiento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion)
        {
            var input = new Domain.AvistamientoFauna.Items.TipoAvistamiento()
            {
                Descripcion = descripcion
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddTipoAvistamiento(input);
        }
    }
}
