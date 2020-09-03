using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.EditarLugarReferencia
{
    public class EditarLugarReferencia : IEditarLugarReferencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarLugarReferencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idLugarReferencia, string descripcion, string latitud, string longitud, string estado)
        {
            var input = new Domain.AvistamientoFauna.Items.LugarReferencia();
            input.IdLugarReferencia = idLugarReferencia;
            input.Descripcion = descripcion;
            input.Latitud = latitud;
            input.Longitud = longitud;
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateLugarReferencia(input);
        }
    }
}
