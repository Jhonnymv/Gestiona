using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportesPreliminares
{
    public class GetReportesPreliminares : IGetReportesPreliminares
    {
        private readonly IGetReportePreliminarReadOnlyRepository _getReportePreliminarReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public GetReportesPreliminares(IGetReportePreliminarReadOnlyRepository getReportePreliminarReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _getReportePreliminarReadOnlyRepository = getReportePreliminarReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportePreliminarReadOnlyRepository));
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorUsuarioReadOnlyRepository));
        }

        public async Task<ReportesPreliminaresOutput> Execute(int tipoIncidente, int nivelIncidente, int gerencia, int asesor, string fecha_inicio, string fecha_fin)
        {

            DateTime? fechaInicioFiltro = null;
            DateTime? fechaFinFiltro = null;

            if (fecha_inicio != null)
                fechaInicioFiltro = Convert.ToDateTime(fecha_inicio);
            if (fecha_fin != null)
                fechaFinFiltro = Convert.ToDateTime(fecha_fin);


            var data = await _getReportePreliminarReadOnlyRepository.GetReportes(fechaInicioFiltro, fechaFinFiltro,tipoIncidente,nivelIncidente,gerencia,asesor);
            data = data.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var output = new ReportesPreliminaresOutput
            {
                Reportes = new List<ReportePreliminarOutput>(),
                NReportes = data.Count
            };

            foreach (var item in data)
            {
                var outputItem = new ReportePreliminarOutput(item);
                output.Reportes.Add(outputItem);
            }

            return output;
        }

        public async Task<ReportesPreliminaresOutput> Execute(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int pagina, int rpp, string usuario_red)
        {
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red); 

            DateTime? fechaInicioFiltro = null;
            DateTime? fechaFinFiltro = null;

            if (fecha_inicio != null)
                fechaInicioFiltro = Convert.ToDateTime(fecha_inicio);
            if (fecha_fin != null)
                fechaFinFiltro = Convert.ToDateTime(fecha_fin);

            var estados = new List<string>();
            if (estado.Length > 0)
            {
                if (estado.Contains("EnProceso"))
                    estados.Add("En Proceso");
                if (estado.Contains("Cerrado"))
                    estados.Add("Cerrado");
            }

            var data = await _getReportePreliminarReadOnlyRepository.GetReportes(fechaInicioFiltro, fechaFinFiltro, gerencia, titular, estados, responsable, asesor, codigo);
            data = data.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var output = new ReportesPreliminaresOutput
            {
                Reportes = new List<ReportePreliminarOutput>(),
                NReportes = data.Count
            };

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                var outputItem = new ReportePreliminarOutput(item);

                if(outputItem.Id_Registrado_Por!=null && outputItem.Id_Registrado_Por == usuario.IdUsuario)
                    outputItem.PermisoEditar = true;

                output.Reportes.Add(outputItem);
            }

            return output;
        }
    }
}
