using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.EliminarObservacion;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarInspeccion
{
    public class EditarInspeccionCabecera : IEditarInspeccionCabecera
    {
        private readonly IEditarInspeccionReadOnlyRepository _editarInspeccionReadonlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnlyRepository;
        private readonly IEliminarObservacion _eliminarObservacion;

        public EditarInspeccionCabecera(IEditarInspeccionReadOnlyRepository editarInspeccionReadonlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository, 
            IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnlyRepository, IEliminarObservacion eliminarObservacion)
        {
            _editarInspeccionReadonlyRepository = editarInspeccionReadonlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadonlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
            _detalleInspeccionReadOnlyRepository = detalleInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(detalleInspeccionReadOnlyRepository));
            _eliminarObservacion = eliminarObservacion ?? throw new ArgumentNullException(nameof(eliminarObservacion));
        }

        public async Task Execute(
            int idCabecera, string objetivo, int gerenciaMarc, int empresaMarc,int gerenciaInspec, string lugar, string tipoInspeccion, int responsableLugar, string hora, string fechaInspeccion, string responsableInpseccion, string inspectores, string opcion, string usuario_red, string codigoFormato, string areaFormato, int? idMiembroComision, string participantesInspeccion)
        {
            var cabecera = new NuevaInspeccionCabecera
            {
                IdCabecera = idCabecera,
                IdGerencia = gerenciaMarc,
                IdEmpresa = empresaMarc,
                IdGerenciaInspeccionada = gerenciaInspec,
                LugarInspeccionado = lugar,
                TipoInspeccion = tipoInspeccion,
                IdResponsableArea = responsableLugar,
                FechaInspeccion = Convert.ToDateTime(fechaInspeccion + " " + hora),
                ResponsableInspeccion = responsableInpseccion,
                Inspectores = inspectores,
                Objetivo = objetivo,
                Area_Formato = areaFormato,
                Codigo_Formato = codigoFormato,
                Id_Miembro_Comite = idMiembroComision,
                Participantes_Inspección = participantesInspeccion
            };

            var Id_Usuario = await _gestorUsuarioReadOnlyRepository.GetIdUsuario(usuario_red);

            switch (opcion)
            {
                case "Eliminar":
                    cabecera.Codigo = "TBD";
                    cabecera.Estado = EstadosInspeccion.Eliminado.ToString();
                    var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(cabecera.IdCabecera);
                    foreach(var observacion in observaciones)
                        await _eliminarObservacion.Execute(observacion.IdInspeccionDetalle, usuario_red);
                    break;
                case "Observacion": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
                case "Guardar":
                    cabecera.Estado = EstadosInspeccion.Registrado.ToString();
                    
                    var codigoAnterior = await _editarInspeccionReadonlyRepository.GetUltimoCodigo();
                    if (codigoAnterior != null)
                    {
                        var correlativo = (Convert.ToInt32(codigoAnterior.Remove(0, 3))+1).ToString().PadLeft(5, '0');                        
                        cabecera.Codigo = "IP-" +correlativo;
                    }
                    else
                        cabecera.Codigo = "IP-00001";
                    await _editarInspeccionReadonlyRepository.ActualizarEstadoBuenasPracticas(idCabecera);
                    break;
                case "GuardarEstado": cabecera.Estado = EstadosInspeccion.Pendiente.ToString(); cabecera.Codigo = "TBD"; break;
                default:
                    cabecera.Estado = null;
                    break;
            }

            await _editarInspeccionReadonlyRepository.EditarInspeccionCabecera(cabecera, Id_Usuario);
        }
    }
}
