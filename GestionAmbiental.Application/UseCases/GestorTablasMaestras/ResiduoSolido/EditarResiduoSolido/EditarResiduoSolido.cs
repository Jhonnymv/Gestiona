using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.EditarResiduoSolido
{
    public class EditarResiduoSolido : IEditarResiduoSolido
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarResiduoSolido(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int Id_Residuo_Solido, int Id_Tipo_Residuo, string Descripcion, string Estado)
        {
            var residuo_solido = new Domain.ValueObjects.Residuo_Solido();
            residuo_solido.Id_Residuo_Solido = Id_Residuo_Solido;
            residuo_solido.Tipo_Residuo = new Domain.ValueObjects.Tipo_Residuo();
            residuo_solido.Tipo_Residuo.Id_Tipo_Residuo = Id_Tipo_Residuo;
            residuo_solido.Descripcion = Descripcion;
            residuo_solido.Estado = Estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateResiduo_Solido(residuo_solido);
        }
    }
}
