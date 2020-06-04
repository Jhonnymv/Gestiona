using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.EditarFactorCausa
{
    public class EditarFactorCausa : IEditarFactorCausa
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarFactorCausa(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string descripcion, string tipo, string estado)
        {
            var input = new Domain.Incidentes.InvestigacionFinal.Factor_Causa_Incidente();
            input.Id_Factor_Causa_Incidente = id;
            input.Descripcion = descripcion;
            input.Tipo_Factor = tipo;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateFactor_Causa_Incidente(input);
        }
    }
}
