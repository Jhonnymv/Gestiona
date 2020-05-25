using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
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

        public GetAccionesIncidente(IGetItemsReportePreliminarReadOnlyRepository getItemsReportePreliminarReadOnlyRepository, IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
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
                outputItem.Accion_Inmediata = item.Accion_Inmediata;
                outputItem.Id_Usuario = item.Id_Usuario;
                if (outputItem.Id_Usuario.HasValue)
                {
                    var usuario = await _gestorTablasMaestrasReadOnlyRepository.GetUsuario(outputItem.Id_Usuario.Value);
                    outputItem.Responsable_Marcobre = usuario.Nombres + " " + usuario.Apellidos;
                }

                outputItem.Responsable_Contratista = item.Responsable;
                outputItem.Fecha = item.Fecha;
                outputItem.Estatus = item.Estatus;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
