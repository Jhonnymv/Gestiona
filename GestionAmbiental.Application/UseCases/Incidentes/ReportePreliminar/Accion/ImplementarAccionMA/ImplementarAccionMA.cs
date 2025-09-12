using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.ImplementarAccionMA
{
    public class ImplementarAccionMA : IImplementarAccionMA
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;
        public ImplementarAccionMA(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }
        public async Task<int> Execute(int IdAccion, List<string> evidencias, string comentario)
        {
            var input = new Accion_Incidente_Reporte();
            input.Id_Reporte_Preliminar = IdAccion;
            input.Evidencia = JsonConvert.SerializeObject(evidencias);
            input.Comentarios_Implementacion = comentario;
            input.Fecha_Implementacion = DateTime.Now;
            input.Comentarios_Verificacion = comentario;
            input.Fecha_Verificacion = DateTime.Now;
            input.Estado = EstadosIncidentes.Aprobado.ToString();

            return await _editarReportePreliminarReadOnlyRepository.ImplementarAccionCorrectivaReportePreliminar(input);
        }
    }
}
