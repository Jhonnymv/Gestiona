using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.EliminarFoto
{
    public class EliminarFoto : IEliminarFoto
    {
        private readonly IEditarAvistamientoFaunaReadOnlyRepository _editarAvistamientoFaunaReadOnlyRepository;

        public EliminarFoto(IEditarAvistamientoFaunaReadOnlyRepository editarAvistamientoFaunaReadOnlyRepository)
        {
            _editarAvistamientoFaunaReadOnlyRepository = editarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarAvistamientoFaunaReadOnlyRepository));
        }

        public async Task Execute(int IdAvistamiento, string foto)
        {
            await _editarAvistamientoFaunaReadOnlyRepository.EliminarFotoAvistamiento(IdAvistamiento, foto);
        }
    }
}
