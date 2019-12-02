using GestionAmbiental.Application.Repositories.GestorUsuarios;
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
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        public AmpliarFechaObservacion(IEditarInspeccionReadOnlyRepository editarInspeccionReadonlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _editarInspeccionReadonlyRepository = editarInspeccionReadonlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadonlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task Execute(int idObservacion, string nuevaFecha,string usuario_red)
        {
            var observacion = new NuevaInspeccionDetalle();
            observacion.Id_Inspeccion_Detalle = idObservacion;
            observacion.Fecha_Programada = Convert.ToDateTime(nuevaFecha);
            observacion.Estado = EstadosInspeccion.Ampliado.ToString();
            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            await _editarInspeccionReadonlyRepository.EditarInspeccionDetalle(observacion, Id_Usuario);
        }
    }
}
