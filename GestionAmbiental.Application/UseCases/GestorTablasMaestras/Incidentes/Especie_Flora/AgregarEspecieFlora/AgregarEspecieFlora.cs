using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Especie_Flora.AgregarEspecieFlora
{
    public class AgregarEspecieFlora : IAgregarEspecieFlora
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarEspecieFlora(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion)
        {
            var item = new Domain.Incidentes.Items.Especie_Flora
            {
                Descripcion = descripcion,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddEspecie_Flora(item);
        }
    }
}
