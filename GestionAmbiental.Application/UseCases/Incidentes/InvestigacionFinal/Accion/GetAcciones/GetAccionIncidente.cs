using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones
{
    public class GetAccionIncidente : IGetAccioIncidenteInvestigacion
    {
        private readonly IGetItemsInvestigacionFinalReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetAccionIncidente(IGetItemsInvestigacionFinalReadOnlyRepository getItemsReportePreliminarReadOnlyRepository, IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<AccioneIncidenteOutput> Execute(int IdAccion)
        {
            var accion = await _getItemsReportePreliminarReadOnlyRepository.GetAccionInvestigacion(IdAccion);
            var output = new AccioneIncidenteOutput();
            output.Id_Accion_Correctiva_Incidente = accion.Id_Accion_Incidente_Investigacion;
            output.Id_Investigacion_Final = accion.Id_Investigacion_Final;
            output.Accion_Inmediata = accion.Accion_Inmediata;
            output.Tipo_Accion = accion.Tipo_Accion;
            output.Id_Usuario = accion.Id_Usuario;
            if (output.Id_Usuario.HasValue)
            {
                var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(output.Id_Usuario.Value);
                output.Responsable_Marcobre = usuario.Nombres + " " + usuario.Apellidos;
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
