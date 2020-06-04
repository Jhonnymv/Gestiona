using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Unidad_Medida.AgregarUnidadMedida
{
    public class AgregarUnidadMedida : IAgregarUnidadMedida
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarUnidadMedida(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion,string tipo)
        {
            var item = new Domain.Incidentes.Items.Unidad_Medida
            {
                Descripcion = descripcion,
                Tipo = tipo,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddUnidad_Medida(item);
        }
    }
}
