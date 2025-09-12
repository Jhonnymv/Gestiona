using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.EditarAccion
{
    public class EditarAccionIncidente : IEditarAccionIncidente
    {
        private readonly IEditarReportePreliminarReadOnlyRepository _editarReportePreliminarReadOnlyRepository;

        public EditarAccionIncidente(IEditarReportePreliminarReadOnlyRepository editarReportePreliminarReadOnlyRepository)
        {
            _editarReportePreliminarReadOnlyRepository = editarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int IdAccion, int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado)
        {
            var fechaAccion = Convert.ToDateTime(fecha);

            var accion = new Accion_Incidente_Reporte();
            accion.Id_Accion_Incidente_Reporte = IdAccion;
            accion.Id_Reporte_Preliminar = IdReporte;
            accion.Accion_Inmediata = descripcion;
            if (usuario != 0)
                accion.Id_Usuario = usuario;
            accion.Responsable = contratista;
            accion.Fecha = fechaAccion;
            accion.Estatus = estado;
            accion.Estado = EstadosIncidentes.Registrado.ToString();

            return await _editarReportePreliminarReadOnlyRepository.EditarAccionCorrectivaReportePreliminar(accion);
        }

        public async Task<int> Execute(int IdAccion, int IdReporte, string Estatus)
        {
            var accion = new Accion_Incidente_Reporte();
            accion.Id_Accion_Incidente_Reporte = IdAccion;
            accion.Id_Reporte_Preliminar = IdReporte;
            accion.Estatus = Estatus;

            return await _editarReportePreliminarReadOnlyRepository.EditarAccionCorrectivaReportePreliminar(accion);
        }
        public async Task<int> Execute(int IdAccion, int IdReporte, string tipoAccion, int IdUsuario, int IdUsuario2, string AccionInmediata, string Fecha, string Responsable, string Estatus)
        {
            var fechaAccion = Convert.ToDateTime(Fecha);

            var accion = new Accion_Incidente_Reporte();
            accion.Id_Accion_Incidente_Reporte = IdAccion;
            accion.Id_Reporte_Preliminar = IdReporte;
            accion.Tipo_Accion = tipoAccion;
            accion.Accion_Inmediata = AccionInmediata;
            if (IdUsuario != 0)
                accion.Id_Usuario = IdUsuario;
            if (IdUsuario2 != 0)
                accion.Id_Usuario2 = IdUsuario2;
            accion.Responsable = Responsable;
            accion.Fecha = fechaAccion;
            accion.Estatus = Estatus;
            accion.Estado = EstadosIncidentes.Registrado.ToString();

            return await _editarReportePreliminarReadOnlyRepository.EditarAccionCorrectivaReportePreliminar(accion);
        }
    }
}
