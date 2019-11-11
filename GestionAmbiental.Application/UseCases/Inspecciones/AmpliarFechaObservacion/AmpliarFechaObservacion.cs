using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.AmpliarFechaObservacion
{
    public class AmpliarFechaObservacion : IAmpliarFechaObservacion
    {
        private readonly IEditarInspeccionReadOnlyRepository _editarInspeccionReadonlyRepository;

        public AmpliarFechaObservacion(IEditarInspeccionReadOnlyRepository editarInspeccionReadonlyRepository)
        {
            _editarInspeccionReadonlyRepository = editarInspeccionReadonlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadonlyRepository));
        }

        public async Task Execute(int idObservacion, string nuevaFecha)
        {
            var observacion = new NuevaInspeccionDetalle();
            observacion.Id_Inspeccion_Detalle = idObservacion;
            observacion.Fecha_Programada = Convert.ToDateTime(nuevaFecha);
            observacion.Estado = EstadosInspeccion.Ampliado.ToString();

            await _editarInspeccionReadonlyRepository.EditarInspeccionDetalle(observacion);
        }
    }
}
