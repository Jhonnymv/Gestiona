using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.EliminarAvistamientoFauna
{
    public class EliminarAvistamientoFauna : IEliminarAvistamientoFauna
    {
        private readonly IEditarAvistamientoFaunaReadOnlyRepository _editarAvistamientoFaunaReadOnlyRepository;

        public EliminarAvistamientoFauna(IEditarAvistamientoFaunaReadOnlyRepository editarAvistamientoFaunaReadOnlyRepository)
        {
            _editarAvistamientoFaunaReadOnlyRepository = editarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarAvistamientoFaunaReadOnlyRepository));
        }

        public async Task Execute(int IdAvistamiento)
        {
            await _editarAvistamientoFaunaReadOnlyRepository.EliminarAvistamiento(IdAvistamiento);
        }
    }
}
