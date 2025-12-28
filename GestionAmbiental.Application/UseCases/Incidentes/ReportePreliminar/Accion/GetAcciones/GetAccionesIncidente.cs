using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones
{
    public class GetAccionesIncidente : IGetAccionesIncidente
    {
        private readonly IGetItemsReportePreliminarReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;

        public GetAccionesIncidente(IGetItemsReportePreliminarReadOnlyRepository getItemsReportePreliminarReadOnlyRepository, IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository, IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
        }
        public async Task<List<AccioneIncidenteOutput>> Execute()
        {
            var items = await _getItemsReportePreliminarReadOnlyRepository.GetAccionesReporte();
            var output = new List<AccioneIncidenteOutput>();
            foreach (var item in items)
            {
                var outputItem = new AccioneIncidenteOutput();
                outputItem.Id_Accion_Correctiva_Incidente = item.Id_Accion_Incidente_Reporte;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                if (outputItem.Id_Usuario.HasValue)
                {
                    var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario.Value);
                    outputItem.Responsable_Marcobre = usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper();
                    outputItem.Responsable_Marcobre_Email = usuario.Email;
                }
                if (outputItem.Id_Usuario2.HasValue)
                {
                    var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario2.Value);
                    outputItem.Responsable_Marcobre2 = usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper();
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
        public async Task<List<AccioneIncidenteOutput>> Execute(int IdReportePreliminar)
        {
            var items = await _getItemsReportePreliminarReadOnlyRepository.GetAccionesReporte(IdReportePreliminar);
            var output = new List<AccioneIncidenteOutput>();
            foreach (var item in items)
            {
                var outputItem = new AccioneIncidenteOutput();
                outputItem.Id_Accion_Correctiva_Incidente = item.Id_Accion_Incidente_Reporte;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                if (outputItem.Id_Usuario.HasValue)
                {
                    var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario.Value);
                    outputItem.Responsable_Marcobre = usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper();
                    outputItem.Responsable_Marcobre_Email = usuario.Email;
                }
                if (outputItem.Id_Usuario2.HasValue)
                {
                    var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario2.Value);
                    outputItem.Responsable_Marcobre2 = usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper();
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
        public async Task<List<AccioneIncidenteOutput>> Execute(int IdReportePreliminar, string usuario_red)
        {
            var items = await _getItemsReportePreliminarReadOnlyRepository.GetAccionesReporte(IdReportePreliminar);
            var info_usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var reporte = await _getReportePreliminarReadOnlyRepository.GetReporte(IdReportePreliminar);

            var output = new List<AccioneIncidenteOutput>();
            foreach (var item in items)
            {
                var outputItem = new AccioneIncidenteOutput();
                outputItem.Id_Accion_Correctiva_Incidente = item.Id_Accion_Incidente_Reporte;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Tipo_Accion = item.Tipo_Accion;
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Id_Usuario2 = item.Id_Usuario2;
                if (outputItem.Id_Usuario.HasValue || outputItem.Id_Usuario2.HasValue)
                {
                    if (outputItem.Id_Usuario.HasValue)
                    {
                        var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario.Value);
                        outputItem.Responsable_Marcobre = usuario.Nombres.ToUpper() + " " + usuario.Apellidos.ToUpper();
                    }
                    if (outputItem.Id_Usuario2.HasValue)
                    {
                        var usuario2 = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario2.Value);
                        outputItem.Responsable_Marcobre2 = usuario2.Nombres.ToUpper() + " " + usuario2.Apellidos.ToUpper();
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

                if (reporte.Supervisor_MA.IdUsuario.Equals(info_usuario.IdUsuario))
                    outputItem.Permiso_Verificacion = true;
                else
                    outputItem.Permiso_Verificacion = false;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
