using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.EliminarInvolucrado
{
    public class EliminarInvolucradoIncidente : IEliminarInvolucradoIncidente
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarInvolucradoIncidente(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task Execute(int IdInvolucradoIncidente)
        {
            var input = new Involucrado_Incidente_Reporte();
            input.Id_Involucrado_Incidente_Reporte = IdInvolucradoIncidente;
            input.Estado = EstadosIncidentes.Eliminado.ToString();
            await _editarReportePreliminarReadOnlyRepository.EditarInvolucradoReportePreliminar(input);
        }
    }
}
