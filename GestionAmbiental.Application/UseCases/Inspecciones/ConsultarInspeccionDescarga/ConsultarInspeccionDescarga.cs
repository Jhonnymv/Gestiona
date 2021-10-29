using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccionDescarga
{
    public class ConsultarInspeccionDescarga : IConsultarInspeccionDescarga
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnlyRepository;

        public ConsultarInspeccionDescarga(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository, IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(cabeceraInspeccionReadOnlyRepository));
            _detalleInspeccionReadOnlyRepository = detalleInspeccionReadOnlyRepository ?? throw new ArgumentNullException(nameof(detalleInspeccionReadOnlyRepository));
        }

        public async Task<InspeccionCabecera> Execute(int idCabecera)
        {
            var cabecera = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraDescarga(idCabecera);
            return cabecera;
        }

        public async Task<List<InspeccionCabecera>> Execute(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado, int idGerenciaInspeccionada)
        {
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
            return await _detalleInspeccionReadOnlyRepository.GetInspeccionesYObservaciones(empresa, gerencia, respLevObs, asesor, fInicio, fFinal,estado,idGerenciaInspeccionada);
        }
    }
}
