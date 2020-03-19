using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.EditarMotivo
{
    public class EditarMotivo : IEditarMotivo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarMotivo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdMotivo, string Descripcion)
        {
            var motivo = new Domain.ValueObjects.Motivo_Ingreso_Salida();
            motivo.Id_Motivo = IdMotivo;
            motivo.Descripcion = Descripcion;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateMotivo_Ingreso_Salida(motivo);
        }
    }
}
