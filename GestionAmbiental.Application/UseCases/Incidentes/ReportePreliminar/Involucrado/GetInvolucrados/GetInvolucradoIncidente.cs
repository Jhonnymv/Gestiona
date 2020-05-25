using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.GetInvolucrados
{
    public class GetInvolucradoIncidente : IGetInvolucradoIncidente
    {
        private readonly IGetItemsReportePreliminarReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;

        public GetInvolucradoIncidente(IGetItemsReportePreliminarReadOnlyRepository getItemsReportePreliminarReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
        }

        public async Task<InvolucradoIncidenteOutput> Execute(int IdInvolucrado)
        {
            var involucrado = await _getItemsReportePreliminarReadOnlyRepository.GetInvolucradoReporte(IdInvolucrado);

            var output = new InvolucradoIncidenteOutput();
            output.Id_Involucrado_Incidente = involucrado.Id_Involucrado_Incidente_Reporte;
            output.Id_Reporte_Preliminar = involucrado.Id_Reporte_Preliminar;
            output.Tipo_Involucrado = involucrado.Tipo_Involucrado;
            output.Id_Usuario = involucrado.Id_Usuario;
            output.Nombres = involucrado.Nombres;
            output.Apellidos = involucrado.Apellidos;
            output.Email = involucrado.Email;
            output.Puesto_Trabajo = involucrado.Puesto_Trabajo;
            output.Celular = involucrado.Celular;
            output.Turno = involucrado.Turno;
            output.Estado = involucrado.Estado;
            return output;
        }
    }
}
