using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Componente_Ambiental.EditarComponentesAmbientales
{
    public class EditarComponenteAmbiental : IEditarComponenteAmbiental
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarComponenteAmbiental(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idComponente, string descripcion, string estado)
        {
            var componente = new Domain.Incidentes.Items.Componente_Ambiental();
            componente.Id_Componente_Ambiental = idComponente;
            componente.Descripcion = descripcion;
            componente.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateComponente_Ambiental(componente);
        }
    }
}
