using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Tipo_Suelo.AgregarTipoSuelo
{
    public class AgregarTipoSuelo : IAgregarTipoSuelo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarTipoSuelo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion)
        {
            var item = new Domain.Incidentes.Items.Tipo_Suelo
            {
                Descripcion = descripcion,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddTipo_Suelo(item);
        }
    }
}
