using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Factor_Causa_Incidente.AgregarFactorCausa
{
    public class AgregarFactorCausa : IAgregarFactorCausa
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarFactorCausa(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion,string tipo)
        {
            var item = new Domain.Incidentes.InvestigacionFinal.Factor_Causa_Incidente
            {
                Descripcion = descripcion,
                Tipo_Factor = tipo,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddFactor_Causa_Incidente(item);
        }
    }
}
