using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Reportes.RAC.GetDetalleReportesXEstado
{
    public class GetDetalleReportesXEstado : IGetDetalleReportesXEstado
    {
        private readonly IGetReportesRACReadOnlyRepository _getReportesRACReadOnlyRepository;
        public GetDetalleReportesXEstado(IGetReportesRACReadOnlyRepository getReportesRACReadOnlyRepository)
        {
            _getReportesRACReadOnlyRepository = getReportesRACReadOnlyRepository ?? throw new ArgumentNullException(nameof(getReportesRACReadOnlyRepository));
        }
        public async Task<ListRacOutput> Execute(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin, string estado)
        {
            //Dar formato a fechas
            DateTime fInicio, fFinal;
            if (FechaInicio != null)
                fInicio = Convert.ToDateTime(FechaInicio);
            else
                fInicio = DateTime.MinValue;

            if (FechaFin != null)
                fFinal = Convert.ToDateTime(FechaFin+" 23:59:59");
            else
                fFinal = DateTime.MaxValue;
            //Obtener valores
            var listaRac = new ListRacOutput();
            listaRac.racs = new List<RacOutput>();
            EstadosRAC estado_domain;
            switch (estado)
            {
                case "Registrado": estado_domain = EstadosRAC.Registrado;break;
                case "Implementado": estado_domain = EstadosRAC.Implementado; break;
                case "Aprobado": estado_domain = EstadosRAC.Aprobado; break;
                case "Rechazado": estado_domain = EstadosRAC.Rechazado; break;
                case "Vencido": estado_domain = EstadosRAC.Vencido; break;
                default:  estado_domain = EstadosRAC.Ampliado;break;

            }
            var data = await _getReportesRACReadOnlyRepository.GetDetalleReportesXEstado(IdGerencia, IdEmpresa, IdUsuario, fInicio, fFinal, estado_domain);
            listaRac.nRacs = data.Count;
            foreach(var item in data)
            {
                listaRac.racs.Add(new RacOutput(item));
            }
            listaRac.racs = listaRac.racs.OrderByDescending(x => x.Codigo).ToList();
            return listaRac;
        }
    }
}
