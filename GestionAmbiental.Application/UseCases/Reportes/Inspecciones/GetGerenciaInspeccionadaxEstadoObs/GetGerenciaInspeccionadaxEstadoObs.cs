using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.Inspecciones.GetGerenciaInspeccionadaxEstadoObs
{
    internal class GetGerenciaInspeccionadaxEstadoObs : IGetGerenciaInspeccionadaxEstadoObs
    {
        private readonly IGetReportesInspeccionesReadOnlyRepository _getReportesInspeccionesReadOnlyRepository;

        public GetGerenciaInspeccionadaxEstadoObs(IGetReportesInspeccionesReadOnlyRepository getReportesInspeccionesReadOnlyRepository)
        {
            _getReportesInspeccionesReadOnlyRepository = getReportesInspeccionesReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesInspeccionesReadOnlyRepository));
        }
        public async Task<List<List<DimensionOutput>>> Execute(int empresa, int gerencia, int respLevObs, int Asesor, string FechaInicio, string FechaFin, string estado, int idGerenciaInspeccionada, int usuarioInvolucrado, string tipoObservacion)
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

            var data = await _getReportesInspeccionesReadOnlyRepository.GetGerenciaInspeccionadaxEstadoObs(empresa, gerencia, respLevObs, Asesor, fInicio, fFinal, estado, idGerenciaInspeccionada, usuarioInvolucrado, tipoObservacion);

            var output = new List<List<DimensionOutput>>();

            foreach (var item in data)
            {
                var temp = new List<DimensionOutput>();
                foreach (var dimen in item)
                {
                    temp.Add(new DimensionOutput{
                        NombrePadre = dimen.NombrePadre,
                        Nombre = dimen.Nombre,
                        Valor = dimen.Valor,
                        Index = dimen.Index
                    });
                }
                output.Add(temp);
            }
            return output;
        }
    }
}
