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
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnlyRepository;

        public AmpliarFechaObservacion(IEditarInspeccionReadOnlyRepository editarInspeccionReadonlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository, IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnlyRepository)
        {
            _editarInspeccionReadonlyRepository = editarInspeccionReadonlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadonlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
            _detalleInspeccionReadOnlyRepository = detalleInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(detalleInspeccionReadOnlyRepository));
        }

        public async Task Execute(int idObservacion, string nuevaFecha,string usuario_red)
        {
            var data_obs = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalle(idObservacion);

            var observacion = new NuevaInspeccionDetalle();
            
            observacion.IdInspeccionDetalle = idObservacion;
            observacion.TipoInspeccion = data_obs.TipoObservacion;
            observacion.IdAsesorMASeguridad = data_obs.Asesor.IdUsuario;
            observacion.DescripcionCondicion = data_obs.DescripcionCondicion;
            observacion.IdTipoCondicion = data_obs.TipoCondicion.IdTipoCondicion;
            observacion.EvidenciaCondicion = data_obs.EvidenciaCondicion;
            observacion.NivelRiesgo = data_obs.NivelRiesgo;
            observacion.DescripcionAccion = data_obs.DescripcionAccion;
            observacion.IdResponsableMarcobre = data_obs.ResponsableMarcobre.IdUsuario;
            observacion.ResponsableContratista = data_obs.ResponsableContratista;
            
            observacion.FechaProgramada = Convert.ToDateTime(nuevaFecha);
            observacion.Estado = EstadosInspeccion.Ampliado.ToString();

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            await _editarInspeccionReadonlyRepository.EditarInspeccionDetalle(observacion, Id_Usuario);
        }
    }
}
