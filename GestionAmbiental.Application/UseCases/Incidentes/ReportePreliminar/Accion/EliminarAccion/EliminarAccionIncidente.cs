using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EliminarAccion;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EliminarAccionIncidente
{
    public class EliminarAccionIncidente : IEliminarAccionIncidente
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EliminarAccionIncidente(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task Execute(int IdAccion)
        {
            var accion = new Accion_Incidente_Reporte();
            accion.Id_Accion_Incidente_Reporte = IdAccion;
            accion.Estado = EstadosIncidentes.Eliminado.ToString();

            await _editarReportePreliminarReadOnlyRepository.EditarAccionCorrectivaReportePreliminar(accion);
        }
    }
}
