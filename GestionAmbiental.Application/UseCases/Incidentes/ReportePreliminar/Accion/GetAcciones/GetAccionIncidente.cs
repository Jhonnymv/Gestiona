using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones
{
    public class GetAccionIncidente : IGetAccioIncidente
    {
        private readonly IGetItemsReportePreliminarReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetAccionIncidente(IGetItemsReportePreliminarReadOnlyRepository getItemsReportePreliminarReadOnlyRepository, IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<AccioneIncidenteOutput> Execute(int IdAccion)
        {
            var accion = await _getItemsReportePreliminarReadOnlyRepository.GetAccionReporte(IdAccion);
            var output = new AccioneIncidenteOutput();
            output.Id_Accion_Correctiva_Incidente = accion.Id_Accion_Incidente_Reporte;
            output.Id_Reporte_Preliminar = accion.Id_Reporte_Preliminar;
            output.Accion_Inmediata = accion.Accion_Inmediata;
            output.Tipo_Accion = accion.Tipo_Accion;
            output.Id_Usuario = accion.Id_Usuario;
            output.Id_Usuario2 = accion.Id_Usuario2;
            if (output.Id_Usuario.HasValue)
            {
                var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(output.Id_Usuario.Value);
                output.Responsable_Marcobre = usuario.Nombres + " " + usuario.Apellidos;
                output.Responsable_Marcobre_Email = usuario.Email;
            }
            if (output.Id_Usuario2.HasValue)
            {
                var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(output.Id_Usuario2.Value);
                output.Responsable_Marcobre2 = usuario.Nombres + " " + usuario.Apellidos;
                output.Responsable_Marcobre2_Email = usuario.Email;
            }

            output.Responsable_Contratista = accion.Responsable;
            output.Fecha = accion.Fecha;
            output.Comentarios_Implementacion = accion.Comentarios_Implementacion;
            output.Fecha_Implementacion = accion.Fecha_Implementacion;
            output.Comentarios_Verificacion = accion.Comentarios_Verificacion;
            output.Fecha_Verificacion = accion.Fecha_Verificacion;
            output.Evidencia = accion.Evidencia;
            output.Estatus = accion.Estatus;
            output.Estado = accion.Estado;

            return output;
        }
    }
}
