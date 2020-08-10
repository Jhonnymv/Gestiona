using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Domain.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.CompletarAvistamientoFauna
{
    public class CompletarAvistamientoFauna : ICompletarAvistamientoFauna
    {
        private readonly IEditarAvistamientoFaunaReadOnlyRepository _editarAvistamientoFaunaReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public CompletarAvistamientoFauna(IEditarAvistamientoFaunaReadOnlyRepository editarAvistamientoFaunaReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarAvistamientoFaunaReadOnlyRepository = editarAvistamientoFaunaReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarAvistamientoFaunaReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int IdAvistamiento, string[] Evidencias, string Descripcion, string UsuarioRed)
        {
            var input = new AvistamientoFauna();

            var IdUsuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(UsuarioRed);

            input.IdAvistamientoFauna = IdAvistamiento;
            input.Evidencia_Accion = JsonConvert.SerializeObject(Evidencias);
            input.Descripcion_Accion = Descripcion;
            input.Supervisor = new Domain.ValueObjects.Usuario { IdUsuario = IdUsuario };
            input.Estado = EstadosAvistamiento.Completado.ToString();

            await _editarAvistamientoFaunaReadOnlyRepository.CompletarAvistamiento(input);
        }
    }
}
