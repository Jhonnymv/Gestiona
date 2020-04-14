using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarAccionCorrectivaIncidente
{
    public class RegistrarAccionCorrectivaIncidente : IRegistrarAccionCorrectivaIncidente
    {
        private readonly IRegistrarReportePreliminarReadOnlyRepository _registrarReportePreliminarReadOnlyRepository;

        public RegistrarAccionCorrectivaIncidente(IRegistrarReportePreliminarReadOnlyRepository registrarReportePreliminarReadOnlyRepository)
        {
            _registrarReportePreliminarReadOnlyRepository = registrarReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarReportePreliminarReadOnlyRepository));
        }

        public async Task<int> Execute(int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado)
        {
            var fechaAccion = Convert.ToDateTime(fecha);

            var accion = new Accion_Correctiva_Incidente();
            accion.Id_Reporte_Preliminar = IdReporte;
            accion.Accion_Inmediata = descripcion;
            if(usuario!=0)
                accion.Id_Usuario = usuario;
            accion.Responsable = contratista;
            accion.Fecha = fechaAccion;
            accion.Estatus = estado;

            return await _registrarReportePreliminarReadOnlyRepository.RegistrarAccionCorrectivaReportePreliminar(accion);
        }
    }
}
