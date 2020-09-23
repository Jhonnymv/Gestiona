using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.EditarOrigenAgua
{
    public class EditarOrigenAgua : IEditarOrigenAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarOrigenAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string descripcion, string estado)
        {
            var input = new Domain.ConsumoAgua.OrigenAgua();
            input.IdOrigenAgua = id;
            input.Descripcion = descripcion;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateOrigenAgua(input);
        }
    }
}
