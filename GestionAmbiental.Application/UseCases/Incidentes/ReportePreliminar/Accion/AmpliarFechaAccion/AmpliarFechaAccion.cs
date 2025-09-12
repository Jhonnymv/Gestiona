using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.AmpliarFechaAccion
{
    public class AmpliarFechaAccion : IAmpliarFechaAccion
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public AmpliarFechaAccion(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAccion, string Fecha)
        {
            var fechaAccion = Convert.ToDateTime(Fecha);

            var accion = new Accion_Incidente_Reporte();
            accion.Id_Accion_Incidente_Reporte = IdAccion;
            accion.Fecha = fechaAccion;
            accion.Estado = EstadosIncidentes.Ampliado.ToString();

            return await _editarReportePreliminarReadOnlyRepository.AmpliarFechaAccion(accion);
        }
    }
}
