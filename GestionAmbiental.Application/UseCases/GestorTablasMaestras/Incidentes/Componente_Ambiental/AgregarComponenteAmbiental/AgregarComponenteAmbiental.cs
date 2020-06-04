using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.AgregarComponenteAmbiental
{
    public class AgregarComponenteAmbiental : IAgregarComponenteAmbiental
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarComponenteAmbiental(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository;
        }

        public async Task<int> Execute(string descripcion)
        {
            var componente = new Domain.Incidentes.Items.Componente_Ambiental
            {
                Descripcion = descripcion,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddComponente_Ambiental(componente);
        }
    }
}
