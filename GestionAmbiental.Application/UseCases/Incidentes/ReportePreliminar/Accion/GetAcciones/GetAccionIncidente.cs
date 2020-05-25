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
            output.Id_Usuario = accion.Id_Usuario;
            if (output.Id_Usuario.HasValue)
            {
                var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(output.Id_Usuario.Value);
                output.Responsable_Marcobre = usuario.Nombres + " " + usuario.Apellidos;
            }

            output.Responsable_Contratista = accion.Responsable;
            output.Fecha = accion.Fecha;
            output.Estatus = accion.Estatus;
            output.Estado = accion.Estado;

            return output;
        }
    }
}
