using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.EliminarAvistamientoFauna
{
    public class EliminarAvistamientoFauna : IEliminarAvistamientoFauna
    {
        private readonly IEditarAvistamientoFaunaReadOnlyRepository _editarAvistamientoFaunaReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public EliminarAvistamientoFauna(IEditarAvistamientoFaunaReadOnlyRepository editarAvistamientoFaunaReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarAvistamientoFaunaReadOnlyRepository = editarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarAvistamientoFaunaReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(string usuarioRed, int IdAvistamiento)
        {
            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuarioRed);
            await _editarAvistamientoFaunaReadOnlyRepository.EliminarAvistamiento(Id_Usuario, IdAvistamiento);
        }
    }
}
