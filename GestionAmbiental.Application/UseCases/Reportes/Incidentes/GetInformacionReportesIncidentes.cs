using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Incidentes
{
    public class GetInformacionReportesIncidentes : IGetInformacionReportesIncidentes
    {
        private readonly IGetReportesIncidentesReadOnlyRepository _repository;

        public GetInformacionReportesIncidentes(IGetReportesIncidentesReadOnlyRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ReportesIncidentesOutput> GetNReportesCausaInmediataIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var data = await _repository.GetNReportesCausasInmediatasIncidente(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new ReportesIncidentesOutput();
            //output.FlashReports = new List<DimensionOutput>();
            output.IncidentesMenores = new List<DimensionOutput>();
            output.IncidentesMayores = new List<DimensionOutput>();

            //foreach (var item in data.FlashReports)
            //{
            //    output.FlashReports.Add(new DimensionOutput(item));
            //}

            foreach (var item in data.IncidentesMenores)
            {
                output.IncidentesMenores.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMayores)
            {
                output.IncidentesMayores.Add(new DimensionOutput(item));
            }
            return output;
        }
        public async Task<ReportesIncidentesOutput> GetNReportesCausaBasicaIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var data = await _repository.GetNReportesCausasBasicasIncidente(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new ReportesIncidentesOutput();
            //output.FlashReports = new List<DimensionOutput>();
            output.IncidentesMenores = new List<DimensionOutput>();
            output.IncidentesMayores = new List<DimensionOutput>();

            //foreach (var item in data.FlashReports)
            //{
            //    output.FlashReports.Add(new DimensionOutput(item));
            //}

            foreach (var item in data.IncidentesMenores)
            {
                output.IncidentesMenores.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMayores)
            {
                output.IncidentesMayores.Add(new DimensionOutput(item));
            }
            return output;
        }

        public async Task<ReportesIncidentesOutput> GetNReportesFecha(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin, string frecuencia)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var data = await _repository.GetNReportes(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal, frecuencia);
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new ReportesIncidentesOutput();
            output.FlashReports = new List<DimensionOutput>();
            output.IncidentesMenores = new List<DimensionOutput>();
            output.IncidentesMayores = new List<DimensionOutput>();

            foreach (var item in data.FlashReports)
            {
                output.FlashReports.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMenores)
            {
                output.IncidentesMenores.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMayores)
            {
                output.IncidentesMayores.Add(new DimensionOutput(item));
            }
            return output;
        }

        public async Task<ReportesIncidentesOutput> GetNReportesGerenciaIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var data = await _repository.GetNReportesGerencia(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new ReportesIncidentesOutput();
            output.FlashReports = new List<DimensionOutput>();
            output.IncidentesMenores = new List<DimensionOutput>();
            output.IncidentesMayores = new List<DimensionOutput>();

            foreach (var item in data.FlashReports)
            {
                output.FlashReports.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMenores)
            {
                output.IncidentesMenores.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMayores)
            {
                output.IncidentesMayores.Add(new DimensionOutput(item));
            }
            return output;
        }

        public async Task<ReportesIncidentesOutput> GetNReportesNivelIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var data = await _repository.GetNReportesNivelIncidente(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new ReportesIncidentesOutput();
            output.FlashReports = new List<DimensionOutput>();
            output.IncidentesMenores = new List<DimensionOutput>();
            output.IncidentesMayores = new List<DimensionOutput>();

            foreach (var item in data.FlashReports)
            {
                output.FlashReports.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMenores)
            {
                output.IncidentesMenores.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMayores)
            {
                output.IncidentesMayores.Add(new DimensionOutput(item));
            }
            output.FlashReports = output.FlashReports.OrderBy(x => x.Nombre).ToList();
            output.IncidentesMenores = output.IncidentesMenores.OrderBy(x => x.Nombre).ToList();
            output.IncidentesMayores = output.IncidentesMayores.OrderBy(x => x.Nombre).ToList();
            return output;
        }

        public async Task<ReportesIncidentesOutput> GetNReportesTipoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, string FechaInicio, string FechaFin)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin + " 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var data = await _repository.GetNReportesTipoIncidente(tipoIncidente, nivelIncidente, gerencia, supervisor, fInicio, fFinal);
            //data = data.OrderBy(x => x.Index).ToList();

            var output = new ReportesIncidentesOutput();
            //output.FlashReports = new List<DimensionOutput>();
            output.IncidentesMenores = new List<DimensionOutput>();
            output.IncidentesMayores = new List<DimensionOutput>();

            //foreach (var item in data.FlashReports)
            //{
            //    output.FlashReports.Add(new DimensionOutput(item));
            //}

            foreach (var item in data.IncidentesMenores)
            {
                output.IncidentesMenores.Add(new DimensionOutput(item));
            }

            foreach (var item in data.IncidentesMayores)
            {
                output.IncidentesMayores.Add(new DimensionOutput(item));
            }
            return output;
        }
    }
}
