using GestionAmbiental.Application.Repositories.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetRACsDescarga
{
    internal class GetRACsDescarga: IGetRACsDescarga
    {
        private readonly IGetReportesRACReadOnlyRepository _getReportesRACReadOnlyRepository;

        public GetRACsDescarga(IGetReportesRACReadOnlyRepository getReportesRACReadOnlyRepository)
        {
            _getReportesRACReadOnlyRepository = getReportesRACReadOnlyRepository;
        }

        public async Task<List<Domain.RAC.RAC>> Execute(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
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
            var data = await _getReportesRACReadOnlyRepository.GetRACs(IdGerencia, IdEmpresa, IdUsuario, fInicio, fFinal);
           
            return data;
        }
    }
}
