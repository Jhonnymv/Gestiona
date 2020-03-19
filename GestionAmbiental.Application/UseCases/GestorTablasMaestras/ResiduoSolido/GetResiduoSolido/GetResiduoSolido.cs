using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduoSolido
{
    public class GetResiduoSolido : IGetResiduoSolido
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetResiduoSolido(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ResiduoSolidoOutput> Execute(int idResiduoSolido)
        {
            var residuo_solido = await _gestorTablasMaestrasReadOnlyRepository.GetResiduo_Solido(idResiduoSolido);
            var output = new ResiduoSolidoOutput(residuo_solido);
            return output;
        }
    }
}
