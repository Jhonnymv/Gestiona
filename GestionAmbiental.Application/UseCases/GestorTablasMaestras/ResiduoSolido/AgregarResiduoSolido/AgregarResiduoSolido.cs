using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.AgregarResiduoSolido
{
    public class AgregarResiduoSolido : IAgregarResiduoSolido
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarResiduoSolido(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(int Id_Tipo_Residuo, string Descripcion)
        {
            var residuo_solido = new Domain.ValueObjects.Residuo_Solido
            {
                Tipo_Residuo = new Domain.ValueObjects.Tipo_Residuo() { Id_Tipo_Residuo = Id_Tipo_Residuo },                
                Descripcion = Descripcion
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddResiduo_Solido(residuo_solido);
        }
    }
}
