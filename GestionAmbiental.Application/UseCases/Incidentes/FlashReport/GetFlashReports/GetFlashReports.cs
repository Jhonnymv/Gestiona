using GestionAmbiental.Application.Repositories.Incidentes.FlashReport;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReports
{
    internal class GetFlashReports : IGetFlashReports
    {
        private readonly IFlashReportReadOnlyRepository _flashReportReadOnlyRepository;

        public GetFlashReports(IFlashReportReadOnlyRepository flashReportReadOnlyRepository)
        {
            _flashReportReadOnlyRepository = flashReportReadOnlyRepository ?? throw new ArgumentNullException(nameof(flashReportReadOnlyRepository));
        }

        public async Task<FlashReportsOutput> Execute(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int pagina, int rpp, string usuarioRed)
        {
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

            var data = await _flashReportReadOnlyRepository.ListarFlashReport(fechaInicioFiltro, fechaFinFiltro, gerencia, titular, estados, responsable, asesor, codigo);
            data = data.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();
            var output = new FlashReportsOutput
            {
                FlashReports = new List<FlashReportOutput>(),
                NFlashReports = data.Count
            };
            data = data.OrderByDescending(x => x.Codigo).ToList();

            data = data.Skip((pagina - 1) * rpp).Take(rpp).ToList();
            foreach (var item in data)
            {
                var outputItem = new FlashReportOutput(item);
                output.FlashReports.Add(outputItem);
            }

            return output;
        }
    }
}
