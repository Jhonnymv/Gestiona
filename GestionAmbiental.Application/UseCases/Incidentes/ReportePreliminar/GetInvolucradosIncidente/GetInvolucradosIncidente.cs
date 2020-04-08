using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetInvolucradosIncidente
{
    public class GetInvolucradosIncidente : IGetInvolucradosIncidente
    {
        private readonly IGetItemsReportePreliminarReadOnlyRepository _getItemsReportePreliminarReadOnlyRepository;

        public GetInvolucradosIncidente(IGetItemsReportePreliminarReadOnlyRepository getItemsReportePreliminarReadOnlyRepository)
        {
            _getItemsReportePreliminarReadOnlyRepository = getItemsReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReportePreliminarReadOnlyRepository));
        }

        public async Task<List<InvolucradoIncidenteOutput>> Execute(int IdReportePreliminar)
        {
            var items = await _getItemsReportePreliminarReadOnlyRepository.GetInvolucradosReporte(IdReportePreliminar);
            var output = new List<InvolucradoIncidenteOutput>();
            foreach (var item in items)
            {
                var outputItem = new InvolucradoIncidenteOutput();
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Tipo_Involucrado = item.Tipo_Involucrado;
                outputItem.Id_Usuario = item.Id_Usuario;
                outputItem.Nombres = item.Nombres;
                outputItem.Apellidos = item.Apellidos;
                outputItem.Email = item.Email;
                outputItem.Puesto_Trabajo = item.Puesto_Trabajo;
                outputItem.Celular = item.Celular;
                outputItem.Turno = item.Turno;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
