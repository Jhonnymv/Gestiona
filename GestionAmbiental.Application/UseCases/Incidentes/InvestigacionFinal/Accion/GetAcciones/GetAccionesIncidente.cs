using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones
{
    public class GetAccionesIncidente : IGetAccionesIncidenteInvestigacion
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        private readonly IGetInvestigacionFinalReadOnlyRepository _getInvestigacionFinalReadOnlyRepository;

        public GetAccionesIncidente(IGetItemsInvestigacionFinalReadOnlyRepository getItemsReportePreliminarReadOnlyRepository, IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository, IGetInvestigacionFinalReadOnlyRepository getInvestigacionFinalReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
            _getInvestigacionFinalReadOnlyRepository = getInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(getInvestigacionFinalReadOnlyRepository));
        }

        public async Task<List<AccioneIncidenteOutput>> Execute(int IdInvestigacion)
        {
            var items = await _getItemsReportePreliminarReadOnlyRepository.GetAccionesInvestigacion(IdInvestigacion);
            var output = new List<AccioneIncidenteOutput>();
            foreach (var item in items)
            {
                var outputItem = new AccioneIncidenteOutput();
                outputItem.Id_Accion_Correctiva_Incidente = item.Id_Accion_Incidente_Investigacion;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                if (outputItem.Id_Usuario.HasValue)
                {
                    var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario.Value);
                    outputItem.Responsable_Marcobre = usuario.Nombres + " " + usuario.Apellidos;
                    outputItem.Responsable_Marcobre_Email = usuario.Email;
                }
                if (outputItem.Id_Usuario2.HasValue)
                {
                    var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario2.Value);
                    outputItem.Responsable_Marcobre2 = usuario.Nombres + " " + usuario.Apellidos;
                    outputItem.Responsable_Marcobre2_Email = usuario.Email;
                }

                outputItem.Responsable_Contratista = item.Responsable;
                outputItem.Fecha = item.Fecha;
                outputItem.Comentarios_Verificacion = item.Comentarios_Verificacion;
                outputItem.Fecha_Verificacion = item.Fecha_Verificacion;
                outputItem.Evidencia = item.Evidencia;
                outputItem.Estatus = item.Estatus;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<AccioneIncidenteOutput>> Execute(int IdInvestigacion, string usuario_red)
        {
            var items = await _getItemsReportePreliminarReadOnlyRepository.GetAccionesInvestigacion(IdInvestigacion);
            var info_usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var investigacion = await _getInvestigacionFinalReadOnlyRepository.GetInvestigacionFinal(IdInvestigacion);

            var output = new List<AccioneIncidenteOutput>();
            foreach (var item in items)
            {
                var outputItem = new AccioneIncidenteOutput();
                outputItem.Id_Accion_Correctiva_Incidente = item.Id_Accion_Incidente_Investigacion;
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                if (outputItem.Id_Usuario.HasValue || outputItem.Id_Usuario2.HasValue)
                {
                    if (outputItem.Id_Usuario.HasValue)
                    {
                        var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario.Value);
                        outputItem.Responsable_Marcobre = usuario.Nombres + " " + usuario.Apellidos;
                    }
                    if (outputItem.Id_Usuario2.HasValue)
                    {
                        var usuario2 = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario2.Value);
                        outputItem.Responsable_Marcobre2 = usuario2.Nombres + " " + usuario2.Apellidos;
                    }

                    if (item.Id_Usuario.Equals(info_usuario.IdUsuario) || item.Id_Usuario2.Equals(info_usuario.IdUsuario))
                        outputItem.Permiso_Implementacion = true;
                    else
                        outputItem.Permiso_Implementacion = false;
                }

                outputItem.Responsable_Contratista = item.Responsable;
                outputItem.Fecha = item.Fecha;
                outputItem.Comentarios_Verificacion = item.Comentarios_Verificacion;
                outputItem.Fecha_Verificacion = item.Fecha_Verificacion;
                outputItem.Evidencia = item.Evidencia;
                outputItem.Estatus = item.Estatus;
                outputItem.Estado = item.Estado;
               
                if(investigacion.Supervisor_MA.IdUsuario.Equals(info_usuario.IdUsuario))
                    outputItem.Permiso_Verificacion = true;
                else
                    outputItem.Permiso_Verificacion = false;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
