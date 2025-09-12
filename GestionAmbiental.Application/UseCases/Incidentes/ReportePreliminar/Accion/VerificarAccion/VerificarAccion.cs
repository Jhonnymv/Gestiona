using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.VerificarAccion
{
    public class VerificarAccion : IVerificarAccion
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;
        public VerificarAccion(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAccion, string Opcion, string Estatus, string Comentario)
        {
            var input = new Accion_Incidente_Reporte();
            input.Id_Reporte_Preliminar = IdAccion;
            input.Estatus = Estatus;
            input.Comentarios_Verificacion = Comentario;
            input.Fecha_Verificacion = DateTime.Now;
            if (Opcion.Equals("Rechazar"))
                input.Estado = EstadosIncidentes.Rechazado.ToString();
            else if (Opcion.Equals("Aprobar"))
            {
                input.Estado = EstadosIncidentes.Aprobado.ToString(); 
                input.Estatus = "Cerrado";
            }

            return await _editarReportePreliminarReadOnlyRepository.VerificarAccionCorrectivaReportePreliminar(input);
        }
    }
}
